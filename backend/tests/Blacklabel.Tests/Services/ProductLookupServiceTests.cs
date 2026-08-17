using Blacklabel.Application.Dtos;
using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;
using Blacklabel.Application.Scoring;
using Blacklabel.Application.Services;
using Blacklabel.Domain.Entities;
using Blacklabel.Domain.Enums;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Repositories;
using Blacklabel.Tests.ExternalClients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Blacklabel.Tests.Services;

public class ProductLookupServiceTests
{
    private static readonly Guid TestUserId = Guid.NewGuid();

    private static (BlacklabelDbContext Context, ProductLookupService Service, FakeOpenFoodFactsClient OffClient) CreateService()
    {
        var options = new DbContextOptionsBuilder<BlacklabelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BlacklabelDbContext(options);
        context.Database.EnsureCreated();

        var offClient = new FakeOpenFoodFactsClient();
        var emptyScopeFactory = new ServiceCollection().BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        var service = new ProductLookupService(
            new ProductRepository(context),
            new AdditiveRepository(context),
            new AllergenRepository(context),
            new UserPreferenceRepository(context),
            new AppUserRepository(context),
            new ScanRepository(context),
            offClient,
            new ScoreCalculator(new ScoreThresholds()),
            emptyScopeFactory,
            NullLogger<ProductLookupService>.Instance);

        return (context, service, offClient);
    }

    private static async Task SeedUserAsync(BlacklabelDbContext context, Guid userId, bool isPremium = false)
    {
        context.AppUsers.Add(new AppUser
        {
            Id = userId,
            DeviceId = $"device-{userId}",
            IsPremium = isPremium,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();
    }

    private static OpenFoodFactsProduct CreateOffProduct(IReadOnlyList<string>? categories = null) => new(
        ProductName: "Test Biscuit",
        ProductNameTr: "Test Bisküvi",
        Brands: "Test Brand",
        Quantity: "200 g",
        IngredientsText: "Wheat flour, sugar, lecithin, sodium benzoate",
        IngredientsTextTr: null,
        AdditivesTags: new List<string> { "E322", "E211" },
        AllergensTags: new List<string> { "gluten", "milk" },
        NovaGroup: 4,
        NutriscoreGrade: "d",
        Nutriments: new NutrimentsDto(
            EnergyKcal100g: 480, Fat100g: 20, SaturatedFat100g: 10, Carbohydrates100g: 60,
            Sugars100g: 28, Fiber100g: 2, Proteins100g: 6, Salt100g: 0.8m),
        ImageUrl: "https://example.org/image.jpg",
        CategoriesTags: categories ?? new List<string> { "biscuits" });

    [Fact]
    public async Task GetByBarcodeAsync_Returns_InvalidBarcode_Without_Calling_OFF()
    {
        var (_, service, offClient) = CreateService();

        var result = await service.GetByBarcodeAsync(TestUserId, "123", CancellationToken.None);

        Assert.Equal(ProductLookupOutcome.InvalidBarcode, result.Outcome);
        Assert.False(result.CanContribute);
        Assert.Null(result.Product);
        Assert.Equal(0, offClient.CallCount);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Returns_NotFound_With_CanContribute_When_OFF_Has_No_Product()
    {
        var (_, service, _) = CreateService();

        var result = await service.GetByBarcodeAsync(TestUserId, "8690504010104", CancellationToken.None);

        Assert.Equal(ProductLookupOutcome.NotFound, result.Outcome);
        Assert.True(result.CanContribute);
        Assert.Null(result.Product);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Fetches_From_OFF_Maps_And_Persists_When_Not_Cached()
    {
        var (context, service, offClient) = CreateService();
        const string barcode = "8690504010104";
        offClient.SetResponse(barcode, CreateOffProduct());

        var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Equal(ProductLookupOutcome.Found, result.Outcome);
        Assert.NotNull(result.Product);
        Assert.Equal("Test Bisküvi", result.Product!.Name);
        Assert.Equal("Test Brand", result.Product.Brand);
        Assert.Equal(ProductSource.OpenFoodFacts.ToString(), result.Product.Source);
        Assert.Contains(result.Product.Additives, a => a.Code == "E322");
        Assert.Contains(result.Product.Additives, a => a.Code == "E211");
        Assert.Contains("gluten", result.Product.Allergens);
        Assert.Contains("milk", result.Product.Allergens);
        Assert.NotNull(result.Product.Score);
        Assert.NotNull(result.Product.ScoreBreakdown);

        var persisted = await context.Products.FirstOrDefaultAsync(p => p.Barcode == barcode);
        Assert.NotNull(persisted);
        Assert.Equal("[\"biscuits\"]", persisted!.Categories);
        Assert.Equal(1, offClient.CallCount);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Returns_Cached_Product_Without_Calling_OFF()
    {
        var (context, service, offClient) = CreateService();
        const string barcode = "8690504010104";

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Barcode = barcode,
            Name = "Cached Product",
            Source = ProductSource.OpenFoodFacts,
            DataQuality = DataQuality.Complete,
            Score = 70,
            ScoreCalculatedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Equal(ProductLookupOutcome.Found, result.Outcome);
        Assert.Equal("Cached Product", result.Product!.Name);
        Assert.Equal(0, offClient.CallCount);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Records_A_Scan_On_Every_Successful_Lookup()
    {
        var (context, service, offClient) = CreateService();
        const string barcode = "8690504010104";
        offClient.SetResponse(barcode, CreateOffProduct());

        await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);
        await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        var scanCount = await context.Scans.CountAsync(s => s.UserId == TestUserId && s.Barcode == barcode);
        Assert.Equal(2, scanCount);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Blocks_Free_User_After_Ten_Scans_Today()
    {
        var (context, service, offClient) = CreateService();
        const string barcode = "8690504010104";
        offClient.SetResponse(barcode, CreateOffProduct());
        await SeedUserAsync(context, TestUserId, isPremium: false);

        for (var i = 0; i < 10; i++)
        {
            var ok = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);
            Assert.Equal(ProductLookupOutcome.Found, ok.Outcome);
        }

        var blocked = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Equal(ProductLookupOutcome.DailyLimitExceeded, blocked.Outcome);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Does_Not_Limit_Premium_Users()
    {
        var (context, service, offClient) = CreateService();
        const string barcode = "8690504010104";
        offClient.SetResponse(barcode, CreateOffProduct());
        await SeedUserAsync(context, TestUserId, isPremium: true);

        for (var i = 0; i < 15; i++)
        {
            var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);
            Assert.Equal(ProductLookupOutcome.Found, result.Outcome);
        }
    }

    [Fact]
    public async Task GetByBarcodeAsync_Returns_Empty_PersonalWarnings_For_Free_User_Even_With_Preferences()
    {
        var (context, service, offClient) = CreateService();
        const string barcode = "8690504010104";
        offClient.SetResponse(barcode, CreateOffProduct());
        await SeedUserAsync(context, TestUserId, isPremium: false);
        context.UserPreferences.Add(new UserPreference
        {
            UserId = TestUserId,
            AllergenCodes = "[\"gluten\"]",
            AvoidedAdditiveCodes = "[]",
            DietFlags = "{}"
        });
        await context.SaveChangesAsync();

        var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Empty(result.Product!.PersonalWarnings);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Returns_PersonalWarnings_For_Premium_User()
    {
        var (context, service, offClient) = CreateService();
        const string barcode = "8690504010104";
        offClient.SetResponse(barcode, CreateOffProduct());
        await SeedUserAsync(context, TestUserId, isPremium: true);
        context.UserPreferences.Add(new UserPreference
        {
            UserId = TestUserId,
            AllergenCodes = "[\"gluten\"]",
            AvoidedAdditiveCodes = "[]",
            DietFlags = "{}"
        });
        await context.SaveChangesAsync();

        var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Contains(result.Product!.PersonalWarnings, w => w.Type == "allergen" && w.Code == "gluten");
    }

    [Fact]
    public async Task GetAlternativesAsync_Returns_PremiumRequired_For_Free_User()
    {
        var (context, service, _) = CreateService();
        await SeedUserAsync(context, TestUserId, isPremium: false);

        var result = await service.GetAlternativesAsync(TestUserId, "8690504010104", CancellationToken.None);

        Assert.Equal(AlternativesOutcome.PremiumRequired, result.Outcome);
        Assert.Empty(result.Alternatives);
    }

    [Fact]
    public async Task GetAlternativesAsync_Returns_Higher_Scored_Products_With_Overlapping_Category()
    {
        var (context, service, _) = CreateService();
        await SeedUserAsync(context, TestUserId, isPremium: true);

        var target = new Product
        {
            Id = Guid.NewGuid(),
            Barcode = "8690504010104",
            Name = "Target",
            Source = ProductSource.OpenFoodFacts,
            DataQuality = DataQuality.Complete,
            Score = 30,
            Categories = "[\"biscuits\"]",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var betterSameCategory = new Product
        {
            Id = Guid.NewGuid(),
            Barcode = "8690504010111",
            Name = "Better Same Category",
            Source = ProductSource.OpenFoodFacts,
            DataQuality = DataQuality.Complete,
            Score = 80,
            Categories = "[\"biscuits\",\"sweet-biscuits\"]",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var betterDifferentCategory = new Product
        {
            Id = Guid.NewGuid(),
            Barcode = "8690504010128",
            Name = "Better Different Category",
            Source = ProductSource.OpenFoodFacts,
            DataQuality = DataQuality.Complete,
            Score = 90,
            Categories = "[\"beverages\"]",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Products.AddRange(target, betterSameCategory, betterDifferentCategory);
        await context.SaveChangesAsync();

        var result = await service.GetAlternativesAsync(TestUserId, target.Barcode, CancellationToken.None);

        Assert.Equal(AlternativesOutcome.Found, result.Outcome);
        Assert.Single(result.Alternatives);
        Assert.Equal("Better Same Category", result.Alternatives[0].Name);
    }
}
