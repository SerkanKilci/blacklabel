using Blacklabel.Application.Interfaces;
using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blacklabel.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly BlacklabelDbContext _context;

    public ProductRepository(BlacklabelDbContext context)
    {
        _context = context;
    }

    public Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken ct)
        => _context.Products
            .Include(p => p.ProductAdditives).ThenInclude(pa => pa.Additive)
            .Include(p => p.ProductAllergens).ThenInclude(pa => pa.Allergen)
            .FirstOrDefaultAsync(p => p.Barcode == barcode, ct);

    public Task<bool> ExistsByBarcodeAsync(string barcode, CancellationToken ct)
        => _context.Products.AnyAsync(p => p.Barcode == barcode, ct);

    public Task<Guid?> GetIdByBarcodeAsync(string barcode, CancellationToken ct)
        => _context.Products.Where(p => p.Barcode == barcode).Select(p => (Guid?)p.Id).FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<Product>> GetTopScoredExcludingAsync(Guid excludeProductId, int minScore, int limit, CancellationToken ct)
        => await _context.Products
            .Include(p => p.ProductAdditives).ThenInclude(pa => pa.Additive)
            .Where(p => p.Id != excludeProductId && p.Score != null && p.Score > minScore)
            .OrderByDescending(p => p.Score)
            .Take(limit)
            .ToListAsync(ct);

    public Task AddAsync(Product product, CancellationToken ct)
    {
        _context.Products.Add(product);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _context.SaveChangesAsync(ct);

    public async Task<Product> AddOrGetExistingAsync(Product product, CancellationToken ct)
    {
        _context.Products.Add(product);
        try
        {
            await _context.SaveChangesAsync(ct);
            return product;
        }
        catch (DbUpdateException)
        {
            // Discard our half-tracked graph (product + its cascaded ProductAdditive/
            // ProductAllergen rows) rather than leaving it Added — a later SaveChangesAsync
            // on this same scoped DbContext (e.g. ScanRepository's, right after this call
            // returns) would otherwise retry inserting it and hit the same conflict again.
            _context.ChangeTracker.Clear();

            var existing = await GetByBarcodeAsync(product.Barcode, ct);
            return existing ?? throw new InvalidOperationException(
                $"Insert of barcode '{product.Barcode}' failed but no existing row was found.");
        }
    }
}
