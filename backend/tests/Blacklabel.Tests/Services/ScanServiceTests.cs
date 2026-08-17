using Blacklabel.Application.Dtos;
using Blacklabel.Application.Services;
using Blacklabel.Domain.Entities;
using Blacklabel.Domain.Enums;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blacklabel.Tests.Services;

public class ScanServiceTests
{
    private static readonly Guid TestUserId = Guid.NewGuid();

    private static (BlacklabelDbContext Context, ScanService Service) CreateService()
    {
        var options = new DbContextOptionsBuilder<BlacklabelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BlacklabelDbContext(options);
        context.Database.EnsureCreated();

        var service = new ScanService(new ScanRepository(context), new ProductRepository(context), new AppUserRepository(context));
        return (context, service);
    }

    private static async Task SeedUserAsync(BlacklabelDbContext context, Guid userId, bool isPremium)
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

    [Fact]
    public async Task RecordScansAsync_Skips_Invalid_Barcodes_But_Records_Valid_Ones()
    {
        var (_, service) = CreateService();

        var requests = new List<CreateScanRequest>
        {
            new("8690504010104", DateTime.UtcNow, 70),
            new("123", DateTime.UtcNow, null),
        };

        var result = await service.RecordScansAsync(TestUserId, requests, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal("8690504010104", result[0].Barcode);
    }

    [Fact]
    public async Task RecordScansAsync_Resolves_ProductId_When_Product_Exists()
    {
        var (context, service) = CreateService();
        const string barcode = "8690504010104";

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Barcode = barcode,
            Name = "Test Product",
            Source = ProductSource.OpenFoodFacts,
            DataQuality = DataQuality.Complete,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var result = await service.RecordScansAsync(
            TestUserId, new[] { new CreateScanRequest(barcode, DateTime.UtcNow, 80) }, CancellationToken.None);

        Assert.Equal(product.Id, result[0].ProductId);
    }

    [Fact]
    public async Task RecordScansAsync_Leaves_ProductId_Null_When_Product_Unknown()
    {
        var (_, service) = CreateService();

        var result = await service.RecordScansAsync(
            TestUserId, new[] { new CreateScanRequest("8690504010104", DateTime.UtcNow, null) }, CancellationToken.None);

        Assert.Null(result[0].ProductId);
    }

    [Fact]
    public async Task GetHistoryAsync_Returns_Paged_Results_Ordered_By_Newest_First()
    {
        var (_, service) = CreateService();

        var now = DateTime.UtcNow;
        await service.RecordScansAsync(TestUserId, new[]
        {
            new CreateScanRequest("8690504010104", now.AddMinutes(-10), 50),
            new CreateScanRequest("8690504010111", now, 60),
        }, CancellationToken.None);

        var page = await service.GetHistoryAsync(TestUserId, 1, 20, CancellationToken.None);

        Assert.Equal(2, page.TotalCount);
        Assert.Equal(2, page.Items.Count);
        Assert.Equal("8690504010111", page.Items[0].Barcode);
    }

    [Fact]
    public async Task GetHistoryAsync_Does_Not_Return_Other_Users_Scans()
    {
        var (_, service) = CreateService();
        var otherUserId = Guid.NewGuid();

        await service.RecordScansAsync(otherUserId, new[]
        {
            new CreateScanRequest("8690504010104", DateTime.UtcNow, 50),
        }, CancellationToken.None);

        var page = await service.GetHistoryAsync(TestUserId, 1, 20, CancellationToken.None);

        Assert.Equal(0, page.TotalCount);
    }

    [Fact]
    public async Task GetHistoryAsync_Caps_Free_User_At_Twenty_Scans()
    {
        var (context, service) = CreateService();
        await SeedUserAsync(context, TestUserId, isPremium: false);

        var requests = Enumerable.Range(0, 25)
            .Select(i => new CreateScanRequest("8690504010104", DateTime.UtcNow.AddMinutes(-i), 50))
            .ToArray();
        await service.RecordScansAsync(TestUserId, requests, CancellationToken.None);

        var page = await service.GetHistoryAsync(TestUserId, 1, 50, CancellationToken.None);

        Assert.Equal(20, page.TotalCount);
        Assert.Equal(20, page.Items.Count);
    }

    [Fact]
    public async Task GetHistoryAsync_Does_Not_Cap_Premium_User()
    {
        var (context, service) = CreateService();
        await SeedUserAsync(context, TestUserId, isPremium: true);

        var requests = Enumerable.Range(0, 25)
            .Select(i => new CreateScanRequest("8690504010104", DateTime.UtcNow.AddMinutes(-i), 50))
            .ToArray();
        await service.RecordScansAsync(TestUserId, requests, CancellationToken.None);

        var page = await service.GetHistoryAsync(TestUserId, 1, 50, CancellationToken.None);

        Assert.Equal(25, page.TotalCount);
        Assert.Equal(25, page.Items.Count);
    }
}
