using System.Text.Json;
using Blacklabel.Application.Scoring;
using Blacklabel.Domain.Enums;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using OffImporter;
using OffImporter.Dump;
using Xunit;

namespace Blacklabel.Tests.Etl;

public class ProductUpsertServiceTests
{
    private static (BlacklabelDbContext Context, ProductUpsertService Service) CreateService()
    {
        var options = new DbContextOptionsBuilder<BlacklabelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BlacklabelDbContext(options);
        context.Database.EnsureCreated();

        var service = new ProductUpsertService(
            context,
            new AdditiveRepository(context),
            new AllergenRepository(context),
            new ScoreCalculator(new ScoreThresholds()));

        return (context, service);
    }

    private static OffDumpProduct ParseDumpLine(string barcode, string countriesTagsJson = "[]") => JsonSerializer.Deserialize<OffDumpProduct>($$"""
        {
            "code": "{{barcode}}",
            "product_name": "Test Biscuit",
            "product_name_tr": "Test Bisküvi",
            "brands": "Test Brand",
            "quantity": "200 g",
            "ingredients_text": "Wheat flour, sugar, lecithin, sodium benzoate",
            "additives_tags": ["en:e322", "en:e211"],
            "allergens_tags": ["en:gluten"],
            "nova_group": 4,
            "nutriscore_grade": "d",
            "nutriments": {
                "energy-kcal_100g": 480,
                "sugars_100g": 28,
                "saturated-fat_100g": 10,
                "salt_100g": 0.8
            },
            "image_url": "https://example.org/image.jpg",
            "categories_tags": ["en:biscuits"],
            "countries_tags": {{countriesTagsJson}}
        }
        """)!;

    [Fact]
    public async Task UpsertAsync_Creates_Product_For_Gs1_Turkey_Prefix()
    {
        var (context, service) = CreateService();
        var raw = ParseDumpLine("8690504010104");

        var result = await service.UpsertAsync(raw, CancellationToken.None);
        await context.SaveChangesAsync();

        Assert.Equal(ProductUpsertService.Result.Created, result);
        var product = await context.Products.Include(p => p.ProductAdditives).SingleAsync();
        Assert.Equal("8690504010104", product.Barcode);
        Assert.Equal(ProductSource.OpenFoodFacts, product.Source);
        Assert.Equal(2, product.ProductAdditives.Count);
        Assert.NotNull(product.Score);
    }

    [Fact]
    public async Task UpsertAsync_Creates_Product_For_Turkey_Country_Tag_With_OutOfRange_Barcode()
    {
        var (context, service) = CreateService();
        var raw = ParseDumpLine("3017620422003", countriesTagsJson: "[\"en:turkey\"]");

        var result = await service.UpsertAsync(raw, CancellationToken.None);
        await context.SaveChangesAsync();

        Assert.Equal(ProductUpsertService.Result.Created, result);
        Assert.Equal(1, await context.Products.CountAsync());
    }

    [Fact]
    public async Task UpsertAsync_Creates_Product_For_European_Country_Tag_With_OutOfRange_Barcode()
    {
        var (context, service) = CreateService();
        var raw = ParseDumpLine("3017620422003", countriesTagsJson: "[\"en:france\"]");

        var result = await service.UpsertAsync(raw, CancellationToken.None);
        await context.SaveChangesAsync();

        Assert.Equal(ProductUpsertService.Result.Created, result);
        Assert.Equal(1, await context.Products.CountAsync());
    }

    [Fact]
    public async Task UpsertAsync_Skips_Product_Outside_Target_Markets()
    {
        var (context, service) = CreateService();
        var raw = ParseDumpLine("3017620422003", countriesTagsJson: "[\"en:japan\"]");

        var result = await service.UpsertAsync(raw, CancellationToken.None);
        await context.SaveChangesAsync();

        Assert.Equal(ProductUpsertService.Result.SkippedNotTargetMarket, result);
        Assert.Equal(0, await context.Products.CountAsync());
    }

    [Fact]
    public async Task UpsertAsync_Skips_Invalid_Barcode()
    {
        var (context, service) = CreateService();
        var raw = ParseDumpLine("123");

        var result = await service.UpsertAsync(raw, CancellationToken.None);
        await context.SaveChangesAsync();

        Assert.Equal(ProductUpsertService.Result.SkippedInvalidBarcode, result);
        Assert.Equal(0, await context.Products.CountAsync());
    }

    [Fact]
    public async Task UpsertAsync_Is_Idempotent_When_Run_Twice_For_Same_Barcode()
    {
        var (context, service) = CreateService();
        var raw = ParseDumpLine("8690504010104");

        var firstResult = await service.UpsertAsync(raw, CancellationToken.None);
        await context.SaveChangesAsync();
        var firstId = (await context.Products.SingleAsync()).Id;

        var secondResult = await service.UpsertAsync(raw, CancellationToken.None);
        await context.SaveChangesAsync();

        Assert.Equal(ProductUpsertService.Result.Created, firstResult);
        Assert.Equal(ProductUpsertService.Result.Updated, secondResult);

        var products = await context.Products.Include(p => p.ProductAdditives).ToListAsync();
        Assert.Single(products);
        Assert.Equal(firstId, products[0].Id);
        Assert.Equal(2, products[0].ProductAdditives.Count);
    }
}
