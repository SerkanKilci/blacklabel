using System.Text.Json;
using Blacklabel.Application.Barcode;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;
using Blacklabel.Application.Mapping;
using Blacklabel.Application.Preferences;
using Blacklabel.Application.Scoring;
using Blacklabel.Domain.Entities;
using Blacklabel.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blacklabel.Application.Services;

public class ProductLookupService : IProductLookupService
{
    private static readonly TimeSpan RefreshAfter = TimeSpan.FromDays(90);
    private const int FreeDailyScanLimit = 10;
    private const int AlternativesCandidatePoolSize = 100;
    private const int AlternativesResultCount = 5;

    private readonly IProductRepository _productRepository;
    private readonly IAdditiveRepository _additiveRepository;
    private readonly IAllergenRepository _allergenRepository;
    private readonly IHouseholdProfileRepository _householdProfileRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IScanRepository _scanRepository;
    private readonly IOpenFoodFactsClient _openFoodFactsClient;
    private readonly ScoreCalculator _scoreCalculator;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProductLookupService> _logger;

    public ProductLookupService(
        IProductRepository productRepository,
        IAdditiveRepository additiveRepository,
        IAllergenRepository allergenRepository,
        IHouseholdProfileRepository householdProfileRepository,
        IAppUserRepository appUserRepository,
        IScanRepository scanRepository,
        IOpenFoodFactsClient openFoodFactsClient,
        ScoreCalculator scoreCalculator,
        IServiceScopeFactory scopeFactory,
        ILogger<ProductLookupService> logger)
    {
        _productRepository = productRepository;
        _additiveRepository = additiveRepository;
        _allergenRepository = allergenRepository;
        _householdProfileRepository = householdProfileRepository;
        _appUserRepository = appUserRepository;
        _scanRepository = scanRepository;
        _openFoodFactsClient = openFoodFactsClient;
        _scoreCalculator = scoreCalculator;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task<ProductLookupResult> GetByBarcodeAsync(Guid userId, string rawBarcode, CancellationToken ct)
    {
        var barcode = BarcodeNormalizer.Normalize(rawBarcode);
        if (barcode is null)
        {
            return new ProductLookupResult(ProductLookupOutcome.InvalidBarcode, false, null);
        }

        var user = await _appUserRepository.GetByIdAsync(userId, ct);
        var isPremium = user?.IsPremium ?? false;

        if (!isPremium)
        {
            var todayCount = await _scanRepository.CountSinceAsync(userId, DateTime.UtcNow.Date, ct);
            if (todayCount >= FreeDailyScanLimit)
            {
                return new ProductLookupResult(ProductLookupOutcome.DailyLimitExceeded, false, null);
            }
        }

        var profiles = await _householdProfileRepository.GetByUserIdAsync(userId, ct);

        var existing = await _productRepository.GetByBarcodeAsync(barcode, ct);
        if (existing is not null)
        {
            if (DateTime.UtcNow - existing.UpdatedAt > RefreshAfter)
            {
                QueueBackgroundRefresh(barcode);
            }

            var cachedAdditives = existing.ProductAdditives.Select(pa => pa.Additive).ToList();
            var cachedResponse = BuildResponse(existing, cachedAdditives, profiles, isPremium);
            await RecordScanAsync(userId, barcode, existing.Id, cachedResponse.Score, ct);
            return new ProductLookupResult(ProductLookupOutcome.Found, false, cachedResponse);
        }

        var offProduct = await _openFoodFactsClient.GetProductAsync(barcode, ct);
        if (offProduct is null)
        {
            return new ProductLookupResult(ProductLookupOutcome.NotFound, true, null);
        }

        var (product, matchedAdditives) = await BuildProductFromOffAsync(barcode, offProduct, ct);
        await _productRepository.AddAsync(product, ct);
        await _productRepository.SaveChangesAsync(ct);

        var response = BuildResponse(product, matchedAdditives, profiles, isPremium);
        await RecordScanAsync(userId, barcode, product.Id, response.Score, ct);
        return new ProductLookupResult(ProductLookupOutcome.Found, false, response);
    }

    public async Task<AlternativesResult> GetAlternativesAsync(Guid userId, string rawBarcode, CancellationToken ct)
    {
        var barcode = BarcodeNormalizer.Normalize(rawBarcode);
        if (barcode is null)
        {
            return new AlternativesResult(AlternativesOutcome.NotFound, Array.Empty<ProductResponse>());
        }

        var user = await _appUserRepository.GetByIdAsync(userId, ct);
        if (user is null || !user.IsPremium)
        {
            return new AlternativesResult(AlternativesOutcome.PremiumRequired, Array.Empty<ProductResponse>());
        }

        var target = await _productRepository.GetByBarcodeAsync(barcode, ct);
        if (target is null || target.Score is null)
        {
            return new AlternativesResult(AlternativesOutcome.NotFound, Array.Empty<ProductResponse>());
        }

        var targetCategories = DeserializeCategories(target.Categories);
        if (targetCategories.Count == 0)
        {
            return new AlternativesResult(AlternativesOutcome.Found, Array.Empty<ProductResponse>());
        }

        var candidates = await _productRepository.GetTopScoredExcludingAsync(
            target.Id, target.Score.Value, AlternativesCandidatePoolSize, ct);

        var alternatives = candidates
            .Where(p => DeserializeCategories(p.Categories).Overlaps(targetCategories))
            .OrderByDescending(p => p.Score)
            .Take(AlternativesResultCount)
            .Select(p => BuildResponse(p, p.ProductAdditives.Select(pa => pa.Additive).ToList(), Array.Empty<HouseholdProfile>(), isPremium: false))
            .ToList();

        return new AlternativesResult(AlternativesOutcome.Found, alternatives);
    }

    private async Task RecordScanAsync(Guid userId, string barcode, Guid? productId, int? score, CancellationToken ct)
    {
        await _scanRepository.AddAsync(new Scan
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Barcode = barcode,
            ProductId = productId,
            ScannedAt = DateTime.UtcNow,
            ScoreAtScanTime = score
        }, ct);
        await _scanRepository.SaveChangesAsync(ct);
    }

    private void QueueBackgroundRefresh(string barcode)
    {
        _ = Task.Run(async () =>
        {
            using var scope = _scopeFactory.CreateScope();
            var offClient = scope.ServiceProvider.GetRequiredService<IOpenFoodFactsClient>();
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            var additiveRepository = scope.ServiceProvider.GetRequiredService<IAdditiveRepository>();
            var allergenRepository = scope.ServiceProvider.GetRequiredService<IAllergenRepository>();

            try
            {
                var refreshed = await offClient.GetProductAsync(barcode, CancellationToken.None);
                if (refreshed is null)
                {
                    return;
                }

                var existing = await productRepository.GetByBarcodeAsync(barcode, CancellationToken.None);
                if (existing is null)
                {
                    return;
                }

                await ApplyOffDataAsync(existing, refreshed, additiveRepository, allergenRepository, CancellationToken.None);
                await productRepository.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Background refresh failed for barcode {Barcode}", barcode);
            }
        }, CancellationToken.None);
    }

    private async Task<(Product Product, IReadOnlyList<Additive> MatchedAdditives)> BuildProductFromOffAsync(
        string barcode, OpenFoodFactsProduct off, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Barcode = barcode,
            CreatedAt = now,
            UpdatedAt = now,
            ProductAdditives = new List<ProductAdditive>(),
            ProductAllergens = new List<ProductAllergen>()
        };

        var matchedAdditives = await ApplyOffDataAsync(product, off, _additiveRepository, _allergenRepository, ct);

        return (product, matchedAdditives);
    }

    private Task<IReadOnlyList<Additive>> ApplyOffDataAsync(
        Product product,
        OpenFoodFactsProduct off,
        IAdditiveRepository additiveRepository,
        IAllergenRepository allergenRepository,
        CancellationToken ct)
        => ProductFromOffMapper.ApplyAsync(product, off, additiveRepository, allergenRepository, _scoreCalculator, ct);

    private ProductResponse BuildResponse(
        Product product, IReadOnlyList<Additive> matchedAdditives, IReadOnlyList<HouseholdProfile> profiles, bool isPremium)
    {
        var nutriments = ProductResponseMapper.DeserializeNutriments(product.Nutriments);
        var riskLevels = matchedAdditives.Select(a => a.RiskLevel).ToList();

        var scoreResult = _scoreCalculator.Calculate(new ScoreInput(
            nutriments?.Sugars100g,
            nutriments?.SaturatedFat100g,
            nutriments?.Salt100g,
            nutriments?.EnergyKcal100g,
            nutriments?.Fiber100g,
            nutriments?.Proteins100g,
            riskLevels,
            product.NovaGroup,
            product.IngredientsText?.Length));

        var response = ProductResponseMapper.ToResponse(product, matchedAdditives, scoreResult);

        // Personal allergen/preference warnings are a premium feature (§11), computed once per
        // household profile so one scan can tell "safe for you" from "not safe for your child" (§CBS/Yuka).
        var additiveCodes = matchedAdditives.Select(a => a.Code).ToList();
        var allergenCodes = product.ProductAllergens.Select(pa => pa.AllergenCode).ToList();
        var profileWarnings = isPremium
            ? profiles
                .Select(p => new ProfileWarningDto(
                    p.Id,
                    p.Name,
                    PersonalWarningCalculator.Calculate(HouseholdProfileMapper.ToPreferenceResponse(p), additiveCodes, allergenCodes, nutriments)))
                .ToList()
            : new List<ProfileWarningDto>();

        return response with { ProfileWarnings = profileWarnings };
    }

    private static HashSet<string> DeserializeCategories(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new HashSet<string>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json)?.ToHashSet() ?? new HashSet<string>();
        }
        catch (JsonException)
        {
            return new HashSet<string>();
        }
    }

}
