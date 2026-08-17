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
}
