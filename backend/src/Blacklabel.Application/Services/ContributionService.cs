using System.Text.Json;
using Blacklabel.Application.Barcode;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;
using Blacklabel.Application.Mapping;
using Blacklabel.Application.Matching;
using Blacklabel.Application.Preferences;
using Blacklabel.Application.Scoring;
using Blacklabel.Domain.Entities;
using Blacklabel.Domain.Enums;

namespace Blacklabel.Application.Services;

public class ContributionService : IContributionService
{
    private const int FreeDailyContributionLimit = 2;

    private readonly IProductRepository _productRepository;
    private readonly IAdditiveRepository _additiveRepository;
    private readonly IAllergenRepository _allergenRepository;
    private readonly IUserPreferenceRepository _userPreferenceRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IContributionRepository _contributionRepository;
    private readonly IImageStorageService _imageStorageService;
    private readonly IVisionService _visionService;
    private readonly ScoreCalculator _scoreCalculator;

    public ContributionService(
        IProductRepository productRepository,
        IAdditiveRepository additiveRepository,
        IAllergenRepository allergenRepository,
        IUserPreferenceRepository userPreferenceRepository,
        IAppUserRepository appUserRepository,
        IContributionRepository contributionRepository,
        IImageStorageService imageStorageService,
        IVisionService visionService,
        ScoreCalculator scoreCalculator)
    {
        _productRepository = productRepository;
        _additiveRepository = additiveRepository;
        _allergenRepository = allergenRepository;
        _userPreferenceRepository = userPreferenceRepository;
        _appUserRepository = appUserRepository;
        _contributionRepository = contributionRepository;
        _imageStorageService = imageStorageService;
        _visionService = visionService;
        _scoreCalculator = scoreCalculator;
    }

    public async Task<ContributionResult> SubmitAsync(
        Guid userId, string rawBarcode, IReadOnlyList<ContributionImage> images, CancellationToken ct)
    {
        var barcode = BarcodeNormalizer.Normalize(rawBarcode);
        if (barcode is null)
        {
            return new ContributionResult(ContributionOutcome.InvalidBarcode, null);
        }

        var user = await _appUserRepository.GetByIdAsync(userId, ct);
        var isPremium = user?.IsPremium ?? false;

        if (!isPremium)
        {
            var todayCount = await _contributionRepository.CountSinceAsync(userId, DateTime.UtcNow.Date, ct);
            if (todayCount >= FreeDailyContributionLimit)
            {
                return new ContributionResult(ContributionOutcome.DailyLimitExceeded, null);
            }
        }

        var preferenceEntity = await _userPreferenceRepository.GetByUserIdAsync(userId, ct);
        var preference = preferenceEntity is null ? null : UserPreferenceMapper.ToResponse(preferenceEntity);

        var contribution = new Contribution
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Barcode = barcode,
            Status = ContributionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var imageUrls = new List<string>();
        foreach (var image in images)
        {
            var url = await _imageStorageService.SaveAsync(contribution.Id, image, ct);
            imageUrls.Add(url);
        }

        contribution.ImageUrls = JsonSerializer.Serialize(imageUrls);

        await _contributionRepository.AddAsync(contribution, ct);
        await _contributionRepository.SaveChangesAsync(ct);

        var extraction = await _visionService.ExtractLabelAsync(images, ct);
        if (extraction is null)
        {
            contribution.Status = ContributionStatus.Failed;
            await _contributionRepository.SaveChangesAsync(ct);
            return new ContributionResult(ContributionOutcome.VisionFailed, null);
        }

        contribution.RawVisionOutput = JsonSerializer.Serialize(extraction);

        var existingProduct = await _productRepository.GetByBarcodeAsync(barcode, ct);
        if (existingProduct is not null)
        {
            // Do not overwrite an existing product from a second contribution; leave it for admin review (§6).
            await _contributionRepository.SaveChangesAsync(ct);

            var existingAdditives = existingProduct.ProductAdditives.Select(pa => pa.Additive).ToList();
            var existingResponse = BuildResponse(existingProduct, existingAdditives, preference, isPremium);
            return new ContributionResult(ContributionOutcome.ExistingProductUnchanged, existingResponse);
        }

        var allAdditives = await _additiveRepository.GetAllAsync(ct);
        var explicitCodes = extraction.AdditiveCodes
            .Select(code => code.Trim().ToUpperInvariant())
            .ToHashSet();
        var synonymCodes = extraction.IngredientsText is null
            ? new HashSet<string>()
            : AdditiveSynonymMatcher.FindCodesByName(extraction.IngredientsText, allAdditives);
        var matchedCodes = explicitCodes.Union(synonymCodes).ToHashSet();
        var matchedAdditives = allAdditives.Where(a => matchedCodes.Contains(a.Code)).ToList();

        var matchedAllergenCodes = new List<string>();
        foreach (var code in extraction.Allergens.Distinct())
        {
            var allergen = await _allergenRepository.GetByCodeAsync(code, ct);
            if (allergen is not null)
            {
                matchedAllergenCodes.Add(allergen.Code);
            }
        }

        var scoreInput = new ScoreInput(
            extraction.Nutriments.Sugars100g,
            extraction.Nutriments.SaturatedFat100g,
            extraction.Nutriments.Salt100g,
            extraction.Nutriments.EnergyKcal100g,
            extraction.Nutriments.Fiber100g,
            extraction.Nutriments.Proteins100g,
            matchedAdditives.Select(a => a.RiskLevel).ToList(),
            NovaGroup: null,
            extraction.IngredientsText?.Length);

        var scoreResult = _scoreCalculator.Calculate(scoreInput);
        var now = DateTime.UtcNow;
        var productId = Guid.NewGuid();

        var rawName = extraction.ProductName ?? string.Empty;
        var name = rawName.Length <= 300 ? rawName : rawName[..300];

        var product = new Product
        {
            Id = productId,
            Barcode = barcode,
            Name = name,
            Brand = Truncate(extraction.Brand, 200),
            Quantity = Truncate(extraction.Quantity, 100),
            IngredientsText = extraction.IngredientsText,
            Nutriments = JsonSerializer.Serialize(extraction.Nutriments),
            Source = ProductSource.Ocr,
            DataQuality = DetermineDataQuality(extraction),
            Score = scoreResult.Score,
            ScoreCalculatedAt = now,
            CreatedAt = now,
            UpdatedAt = now,
            ProductAdditives = matchedAdditives
                .Select(a => new ProductAdditive { ProductId = productId, AdditiveCode = a.Code })
                .ToList(),
            ProductAllergens = matchedAllergenCodes
                .Select(code => new ProductAllergen { ProductId = productId, AllergenCode = code })
                .ToList()
        };

        await _productRepository.AddAsync(product, ct);
        await _productRepository.SaveChangesAsync(ct);

        contribution.Status = ContributionStatus.Processed;
        await _contributionRepository.SaveChangesAsync(ct);

        var response = BuildResponse(product, matchedAdditives, preference, isPremium);
        return new ContributionResult(ContributionOutcome.Created, response);
    }

    private ProductResponse BuildResponse(
        Product product, IReadOnlyList<Additive> matchedAdditives, UserPreferenceResponse? preference, bool isPremium)
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

        var warnings = isPremium
            ? PersonalWarningCalculator.Calculate(
                preference,
                matchedAdditives.Select(a => a.Code).ToList(),
                product.ProductAllergens.Select(pa => pa.AllergenCode).ToList(),
                nutriments)
            : Array.Empty<PersonalWarningDto>();

        return response with { PersonalWarnings = warnings };
    }

    private static DataQuality DetermineDataQuality(LabelExtractionResult extraction)
    {
        if (extraction.Confidence < 0.6)
        {
            return DataQuality.Unverified;
        }

        var populatedNutrimentCount = new[]
        {
            extraction.Nutriments.EnergyKcal100g,
            extraction.Nutriments.SaturatedFat100g,
            extraction.Nutriments.Sugars100g,
            extraction.Nutriments.Salt100g,
            extraction.Nutriments.Fiber100g,
            extraction.Nutriments.Proteins100g
        }.Count(value => value.HasValue);

        var hasIngredients = !string.IsNullOrWhiteSpace(extraction.IngredientsText);

        return hasIngredients && populatedNutrimentCount >= 4 ? DataQuality.Complete : DataQuality.Partial;
    }

    private static string? Truncate(string? value, int maxLength)
        => string.IsNullOrEmpty(value) ? value : (value.Length <= maxLength ? value : value[..maxLength]);
}
