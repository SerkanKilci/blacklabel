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
using Xunit;

namespace Blacklabel.Tests.Services;

public class ContributionServiceTests
{
    private static readonly Guid TestUserId = Guid.NewGuid();

    private static IReadOnlyList<ContributionImage> CreateImages() => new List<ContributionImage>
    {
        new("front", "front.jpg", "image/jpeg", new byte[] { 1, 2, 3 }),
        new("ingredients", "ingredients.jpg", "image/jpeg", new byte[] { 4, 5, 6 }),
        new("nutrition", "nutrition.jpg", "image/jpeg", new byte[] { 7, 8, 9 }),
    };

    private static LabelExtractionResult CreateExtraction(double confidence = 0.9) => new(
        ProductName: "Ev Yapımı Bisküvi",
        Brand: "Test Brand",
        Quantity: "150 g",
        IngredientsText: "Buğday unu, şeker, lesitin, sodyum benzoat.",
        AdditiveCodes: new List<string> { "e211" },
        Allergens: new List<string> { "gluten" },
        Nutriments: new NutrimentsDto(450, 15, 8, 65, 25, 3, 6, 0.9m),
        Confidence: confidence);

    private static (BlacklabelDbContext Context, ContributionService Service) CreateService(LabelExtractionResult? extraction)
    {
        var options = new DbContextOptionsBuilder<BlacklabelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BlacklabelDbContext(options);
        context.Database.EnsureCreated();

        var service = new ContributionService(
            new ProductRepository(context),
            new AdditiveRepository(context),
            new AllergenRepository(context),
            new HouseholdProfileRepository(context),
            new AppUserRepository(context),
            new ContributionRepository(context),
            new FakeImageStorageService(),
            new FakeVisionService(extraction),
            new ScoreCalculator(new ScoreThresholds()));

        return (context, service);
    }

    private static async Task SeedPremiumUserAsync(BlacklabelDbContext context, Guid userId)
    {
        context.AppUsers.Add(new AppUser
        {
            Id = userId,
            DeviceId = $"device-{userId}",
            IsPremium = true,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task SubmitAsync_Returns_InvalidBarcode_For_Bad_Barcode()
    {
        var (_, service) = CreateService(CreateExtraction());

        var result = await service.SubmitAsync(TestUserId, "123", CreateImages(), CancellationToken.None);

        Assert.Equal(ContributionOutcome.InvalidBarcode, result.Outcome);
        Assert.Null(result.Product);
    }

    [Fact]
    public async Task SubmitAsync_Marks_Contribution_Failed_When_Vision_Returns_Null()
    {
        var (context, service) = CreateService(null);
        const string barcode = "8690504010104";

        var result = await service.SubmitAsync(TestUserId, barcode, CreateImages(), CancellationToken.None);

        Assert.Equal(ContributionOutcome.VisionFailed, result.Outcome);
        Assert.Null(result.Product);

        var contribution = await context.Contributions.FirstAsync(c => c.Barcode == barcode);
        Assert.Equal(ContributionStatus.Failed, contribution.Status);
    }

    [Fact]
    public async Task SubmitAsync_Creates_Product_Matching_Explicit_And_Synonym_Additives()
    {
        var (context, service) = CreateService(CreateExtraction());
        const string barcode = "8690504010104";

        var result = await service.SubmitAsync(TestUserId, barcode, CreateImages(), CancellationToken.None);

        Assert.Equal(ContributionOutcome.Created, result.Outcome);
        Assert.NotNull(result.Product);
        Assert.Equal(ProductSource.Ocr.ToString(), result.Product!.Source);
        // E211 comes from the explicit additiveCodes list, E322 (lecithin) from ingredient-text synonym matching.
        Assert.Contains(result.Product.Additives, a => a.Code == "E211");
        Assert.Contains(result.Product.Additives, a => a.Code == "E322");
        Assert.Contains("gluten", result.Product.Allergens);

        var contribution = await context.Contributions.FirstAsync(c => c.Barcode == barcode);
        Assert.Equal(ContributionStatus.Processed, contribution.Status);

        var product = await context.Products.FirstAsync(p => p.Barcode == barcode);
        Assert.Equal(DataQuality.Complete, product.DataQuality);
    }

    [Fact]
    public async Task SubmitAsync_Marks_Product_Unverified_When_Confidence_Is_Low()
    {
        var (context, service) = CreateService(CreateExtraction(confidence: 0.4));
        const string barcode = "8690504010104";

        await service.SubmitAsync(TestUserId, barcode, CreateImages(), CancellationToken.None);

        var product = await context.Products.FirstAsync(p => p.Barcode == barcode);
        Assert.Equal(DataQuality.Unverified, product.DataQuality);
    }

    [Fact]
    public async Task SubmitAsync_Does_Not_Overwrite_Existing_Product_But_Still_Records_Contribution()
    {
        var (context, service) = CreateService(CreateExtraction());
        const string barcode = "8690504010104";

        var existingProduct = new Product
        {
            Id = Guid.NewGuid(),
            Barcode = barcode,
            Name = "Original Product",
            Source = ProductSource.OpenFoodFacts,
            DataQuality = DataQuality.Complete,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Products.Add(existingProduct);
        await context.SaveChangesAsync();

        var result = await service.SubmitAsync(TestUserId, barcode, CreateImages(), CancellationToken.None);

        Assert.Equal(ContributionOutcome.ExistingProductUnchanged, result.Outcome);
        Assert.Equal("Original Product", result.Product!.Name);

        var products = await context.Products.Where(p => p.Barcode == barcode).ToListAsync();
        Assert.Single(products);
        Assert.Equal("Original Product", products[0].Name);

        var contribution = await context.Contributions.FirstAsync(c => c.Barcode == barcode);
        Assert.Equal(ContributionStatus.Pending, contribution.Status);
    }

    [Fact]
    public async Task SubmitAsync_Blocks_Free_User_After_Two_Contributions_Today()
    {
        var (_, service) = CreateService(CreateExtraction());

        var first = await service.SubmitAsync(TestUserId, "8690504010104", CreateImages(), CancellationToken.None);
        var second = await service.SubmitAsync(TestUserId, "8690504010111", CreateImages(), CancellationToken.None);
        var third = await service.SubmitAsync(TestUserId, "8690504010128", CreateImages(), CancellationToken.None);

        Assert.Equal(ContributionOutcome.Created, first.Outcome);
        Assert.Equal(ContributionOutcome.Created, second.Outcome);
        Assert.Equal(ContributionOutcome.DailyLimitExceeded, third.Outcome);
    }

    [Fact]
    public async Task SubmitAsync_Does_Not_Limit_Premium_Users()
    {
        var (context, service) = CreateService(CreateExtraction());
        await SeedPremiumUserAsync(context, TestUserId);

        var first = await service.SubmitAsync(TestUserId, "8690504010104", CreateImages(), CancellationToken.None);
        var second = await service.SubmitAsync(TestUserId, "8690504010111", CreateImages(), CancellationToken.None);
        var third = await service.SubmitAsync(TestUserId, "8690504010128", CreateImages(), CancellationToken.None);

        Assert.Equal(ContributionOutcome.Created, first.Outcome);
        Assert.Equal(ContributionOutcome.Created, second.Outcome);
        Assert.Equal(ContributionOutcome.Created, third.Outcome);
    }
}
