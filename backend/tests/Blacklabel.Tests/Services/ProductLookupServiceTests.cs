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

    private static (BlacklabelDbContext Context, ProductLookupService Service, FakeOpenFoodFactsClient OffClient, FakeUsdaFoodDataClient UsdaClient) CreateService()
    {
        var options = new DbContextOptionsBuilder<BlacklabelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BlacklabelDbContext(options);
        context.Database.EnsureCreated();

        var offClient = new FakeOpenFoodFactsClient();
        var usdaClient = new FakeUsdaFoodDataClient();
        var emptyScopeFactory = new ServiceCollection().BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        var service = new ProductLookupService(
            new ProductRepository(context),
            new AdditiveRepository(context),
            new AllergenRepository(context),
            new HouseholdProfileRepository(context),
            new AppUserRepository(context),
            new ScanRepository(context),
            offClient,
            usdaClient,
            new ScoreCalculator(new ScoreThresholds()),
            emptyScopeFactory,
            NullLogger<ProductLookupService>.Instance);

        return (context, service, offClient, usdaClient);
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

    // Missing ingredients (the real-world OFF gap this feature targets) -- nutriments are fully
    // populated so DataQuality.Partial is driven specifically by hasIngredients being false.
    private static OpenFoodFactsProduct CreatePartialOffProduct() => new(
        ProductName: "Test Cereal",
        ProductNameTr: null,
        Brands: "Test Brand",
        Quantity: "300 g",
        IngredientsText: null,
        IngredientsTextTr: null,
        AdditivesTags: new List<string>(),
        AllergensTags: new List<string>(),
        NovaGroup: 3,
        NutriscoreGrade: "c",
        Nutriments: new NutrimentsDto(
            EnergyKcal100g: 380, Fat100g: 5, SaturatedFat100g: 1, Carbohydrates100g: 70,
            Sugars100g: 10, Fiber100g: 6, Proteins100g: 8, Salt100g: 1.0m),
        ImageUrl: null,
        CategoriesTags: new List<string> { "cereals" });

    private static UsdaFoodItem CreateUsdaMatch(string gtinUpc) => new(
        GtinUpc: gtinUpc,
        IngredientsText: "WHOLE GRAIN OATS, SUGAR, SALT",
        Nutriments: new NutrimentsDto(
            EnergyKcal100g: 379, Fat100g: 4.8m, SaturatedFat100g: 0.9m, Carbohydrates100g: 71,
            Sugars100g: 9.5m, Fiber100g: 6.2m, Proteins100g: 8.1m, Salt100g: 1.1m));

    [Fact]
    public async Task GetByBarcodeAsync_Enriches_From_Usda_When_Off_Is_Partial_On_A_Us_Barcode()
    {
        var (context, service, offClient, usdaClient) = CreateService();
        const string barcode = "0016000275287"; // US GS1 prefix (001)
        offClient.SetResponse(barcode, CreatePartialOffProduct());
        usdaClient.SetResponse(barcode, CreateUsdaMatch("00016000275287"));

        var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Equal(ProductLookupOutcome.Found, result.Outcome);
        Assert.Equal("WHOLE GRAIN OATS, SUGAR, SALT", result.Product!.IngredientsText);
        Assert.Equal(1, usdaClient.CallCount);

        // Still Partial: USDA has no structured allergen signal equivalent to OFF's
        // allergens_tags, so a product "completed" this way must not claim DataQuality.Complete
        // and silence the incomplete-data safety warning.
        Assert.Equal(DataQuality.Partial.ToString(), result.Product.DataQuality);

        var persisted = await context.Products.FirstAsync(p => p.Barcode == barcode);
        Assert.NotNull(persisted.UsdaEnrichedAt);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Is_Complete_With_Full_Ingredients_Even_When_Most_Nutriments_Are_Missing()
    {
        // Regression test: DataQuality.Complete must depend only on ingredients being present, not
        // on how many nutriment fields OFF's contributors happened to fill in -- the "data
        // incomplete" safety warning is about allergen/additive trustworthiness, which ingredients
        // alone determine. Tying it to nutriment completeness was flagging ~800K real products
        // (full ingredients, sparse nutrition facts) as incomplete.
        var (_, service, offClient, _) = CreateService();
        const string barcode = "8690504010104";
        var sparseNutrimentsProduct = CreateOffProduct() with
        {
            Nutriments = new NutrimentsDto(
                EnergyKcal100g: 480, Fat100g: null, SaturatedFat100g: null, Carbohydrates100g: null,
                Sugars100g: null, Fiber100g: null, Proteins100g: null, Salt100g: null)
        };
        offClient.SetResponse(barcode, sparseNutrimentsProduct);

        var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Equal(DataQuality.Complete.ToString(), result.Product!.DataQuality);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Is_Partial_When_Allergens_Tags_Empty_But_Ingredients_Mention_One()
    {
        // OFF's own allergens_tags came back empty, but the free-text ingredients clearly name a
        // known allergen (milk) -- must not claim Complete and silently imply "nothing flagged".
        var (_, service, offClient, _) = CreateService();
        const string barcode = "8690504010104";
        var untaggedAllergenProduct = CreateOffProduct() with
        {
            IngredientsText = "Wheat flour, sugar, milk powder, salt",
            AllergensTags = new List<string>()
        };
        offClient.SetResponse(barcode, untaggedAllergenProduct);

        var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Equal(DataQuality.Partial.ToString(), result.Product!.DataQuality);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Skips_Usda_When_Off_Data_Is_Already_Complete()
    {
        var (_, service, offClient, usdaClient) = CreateService();
        const string barcode = "0016000275287";
        offClient.SetResponse(barcode, CreateOffProduct());

        await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Equal(0, usdaClient.CallCount);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Skips_Usda_For_A_Barcode_Outside_Its_Coverage()
    {
        var (_, service, offClient, usdaClient) = CreateService();
        const string barcode = "8690504010104"; // Turkey GS1 prefix -- FDC doesn't cover this market
        offClient.SetResponse(barcode, CreatePartialOffProduct());

        await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Equal(0, usdaClient.CallCount);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Marks_UsdaEnrichedAt_Even_When_Usda_Has_No_Match_So_It_Is_Not_Requeried()
    {
        var (context, service, offClient, usdaClient) = CreateService();
        const string barcode = "0016000275287";
        offClient.SetResponse(barcode, CreatePartialOffProduct());
        // No usdaClient.SetResponse -- FakeUsdaFoodDataClient defaults to NotFound.

        await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        var persisted = await context.Products.FirstAsync(p => p.Barcode == barcode);
        Assert.NotNull(persisted.UsdaEnrichedAt);
        Assert.Equal(DataQuality.Partial.ToString(), (await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None)).Product!.DataQuality);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Returns_InvalidBarcode_Without_Calling_OFF()
    {
        var (_, service, offClient, _) = CreateService();

        var result = await service.GetByBarcodeAsync(TestUserId, "123", CancellationToken.None);

        Assert.Equal(ProductLookupOutcome.InvalidBarcode, result.Outcome);
        Assert.False(result.CanContribute);
        Assert.Null(result.Product);
        Assert.Equal(0, offClient.CallCount);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Returns_NotFound_With_CanContribute_When_OFF_Has_No_Product()
    {
        var (_, service, _, _) = CreateService();

        var result = await service.GetByBarcodeAsync(TestUserId, "8690504010104", CancellationToken.None);

        Assert.Equal(ProductLookupOutcome.NotFound, result.Outcome);
        Assert.True(result.CanContribute);
        Assert.Null(result.Product);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Returns_LookupUnavailable_Rather_Than_NotFound_When_OFF_Is_Unreachable()
    {
        var (_, service, offClient, _) = CreateService();
        const string barcode = "8690504010104";
        offClient.SetUnavailable(barcode);

        var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        // Must not be reported as NotFound: the product may well exist, we just couldn't check
        // (rate-limited, OFF down, timeout) -- telling the user "not in our database" would be a
        // false negative and, pre-this-fix, would have offered them a (now-disabled) contribute flow.
        Assert.Equal(ProductLookupOutcome.LookupUnavailable, result.Outcome);
        Assert.Null(result.Product);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Fetches_From_OFF_Maps_And_Persists_When_Not_Cached()
    {
        var (context, service, offClient, _) = CreateService();
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
        var (context, service, offClient, _) = CreateService();
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
        var (context, service, offClient, _) = CreateService();
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
        var (context, service, offClient, _) = CreateService();
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
        var (context, service, offClient, _) = CreateService();
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
    public async Task GetByBarcodeAsync_Returns_Empty_ProfileWarnings_For_Free_User_Even_With_Profiles()
    {
        var (context, service, offClient, _) = CreateService();
        const string barcode = "8690504010104";
        offClient.SetResponse(barcode, CreateOffProduct());
        await SeedUserAsync(context, TestUserId, isPremium: false);
        context.HouseholdProfiles.Add(new HouseholdProfile
        {
            Id = Guid.NewGuid(),
            UserId = TestUserId,
            Name = "Ben",
            AllergenCodes = "[\"gluten\"]",
            AvoidedAdditiveCodes = "[]",
            DietFlags = "{}"
        });
        await context.SaveChangesAsync();

        var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Empty(result.Product!.ProfileWarnings);
    }

    [Fact]
    public async Task GetByBarcodeAsync_Returns_ProfileWarnings_For_Premium_User()
    {
        var (context, service, offClient, _) = CreateService();
        const string barcode = "8690504010104";
        offClient.SetResponse(barcode, CreateOffProduct());
        await SeedUserAsync(context, TestUserId, isPremium: true);
        context.HouseholdProfiles.Add(new HouseholdProfile
        {
            Id = Guid.NewGuid(),
            UserId = TestUserId,
            Name = "Ben",
            AllergenCodes = "[\"gluten\"]",
            AvoidedAdditiveCodes = "[]",
            DietFlags = "{}"
        });
        await context.SaveChangesAsync();

        var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        var profileWarning = Assert.Single(result.Product!.ProfileWarnings);
        Assert.Equal("Ben", profileWarning.ProfileName);
        Assert.Contains(profileWarning.Warnings, w => w.Type == "allergen" && w.Code == "gluten");
    }

    [Fact]
    public async Task GetByBarcodeAsync_Computes_Independent_Warnings_Per_Household_Profile()
    {
        var (context, service, offClient, _) = CreateService();
        const string barcode = "8690504010104";
        offClient.SetResponse(barcode, CreateOffProduct());
        await SeedUserAsync(context, TestUserId, isPremium: true);
        context.HouseholdProfiles.AddRange(
            new HouseholdProfile { Id = Guid.NewGuid(), UserId = TestUserId, Name = "Ahmet", AllergenCodes = "[\"gluten\"]" },
            new HouseholdProfile { Id = Guid.NewGuid(), UserId = TestUserId, Name = "Ayşe", AllergenCodes = "[\"peanuts\"]" });
        await context.SaveChangesAsync();

        var result = await service.GetByBarcodeAsync(TestUserId, barcode, CancellationToken.None);

        Assert.Equal(2, result.Product!.ProfileWarnings.Count);
        var ahmet = result.Product.ProfileWarnings.Single(pw => pw.ProfileName == "Ahmet");
        var ayse = result.Product.ProfileWarnings.Single(pw => pw.ProfileName == "Ayşe");
        Assert.Contains(ahmet.Warnings, w => w.Type == "allergen" && w.Code == "gluten");
        Assert.Empty(ayse.Warnings);
    }

    [Fact]
    public async Task GetAlternativesAsync_Returns_PremiumRequired_For_Free_User()
    {
        var (context, service, _, _) = CreateService();
        await SeedUserAsync(context, TestUserId, isPremium: false);

        var result = await service.GetAlternativesAsync(TestUserId, "8690504010104", CancellationToken.None);

        Assert.Equal(AlternativesOutcome.PremiumRequired, result.Outcome);
        Assert.Empty(result.Alternatives);
    }

    [Fact]
    public async Task GetAlternativesAsync_Returns_Higher_Scored_Products_With_Overlapping_Category()
    {
        var (context, service, _, _) = CreateService();
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
