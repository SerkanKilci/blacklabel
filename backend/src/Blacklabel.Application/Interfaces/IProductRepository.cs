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
}
