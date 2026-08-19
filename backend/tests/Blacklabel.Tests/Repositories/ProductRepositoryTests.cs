using Blacklabel.Domain.Entities;
using Blacklabel.Domain.Enums;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blacklabel.Tests.Repositories;

public class ProductRepositoryTests
{
    private static BlacklabelDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<BlacklabelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BlacklabelDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private static Product CreateProduct(string barcode, string name)
    {
        var now = DateTime.UtcNow;
        return new Product
        {
            Id = Guid.NewGuid(),
            Barcode = barcode,
            Name = name,
            Source = ProductSource.OpenFoodFacts,
            DataQuality = DataQuality.Complete,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    [Fact]
    public async Task AddOrGetExistingAsync_Inserts_And_Returns_The_Same_Instance_When_No_Conflict()
    {
        using var context = CreateContext();
        var repository = new ProductRepository(context);
        var product = CreateProduct("8690504010104", "First Insert");

        var result = await repository.AddOrGetExistingAsync(product, CancellationToken.None);

        Assert.Same(product, result);
        Assert.Equal(1, await context.Products.CountAsync());
    }

    // The race AddOrGetExistingAsync exists for (two concurrent requests both missing the cache
    // for a never-before-seen barcode, both inserting) isn't covered by an automated test here:
    // confirmed directly that EF Core 8's InMemory provider does not enforce HasIndex(...)
    // .IsUnique() at all — two rows with the same Barcode both save successfully under InMemory,
    // same or separate DbContext instances. The catch (DbUpdateException) is correct for the real
    // target (SQL Server enforces the index and EF Core's documented SaveChanges contract wraps
    // any relational provider's constraint violation in DbUpdateException) — it just can't be
    // exercised by this InMemory-only test suite without adding a second, SQLite-backed provider
    // just for this one case. Verify with a real integration test against SQL Server, or manually
    // (two near-simultaneous requests for the same brand-new barcode), before relying on it.
}
