using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken ct);
    Task<bool> ExistsByBarcodeAsync(string barcode, CancellationToken ct);
    Task<Guid?> GetIdByBarcodeAsync(string barcode, CancellationToken ct);
    Task<IReadOnlyList<Product>> GetTopScoredExcludingAsync(Guid excludeProductId, int minScore, int limit, CancellationToken ct);
    Task AddAsync(Product product, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);

    /// <summary>
    /// Inserts <paramref name="product"/>, or — if a concurrent request already inserted a
    /// product with the same barcode first (e.g. two users scanning a never-before-seen barcode
    /// at the same moment, both missing the cache and both fetching it from Open Food Facts) —
    /// discards it and returns the existing row instead. Never throws on that race; the barcode
    /// unique index is the source of truth for "who won".
    /// </summary>
    Task<Product> AddOrGetExistingAsync(Product product, CancellationToken ct);
}
