using Blacklabel.Application.Dtos;

namespace Blacklabel.Application.Interfaces;

public enum ProductLookupOutcome
{
    Found,
    NotFound,
    InvalidBarcode,
    DailyLimitExceeded
}

public sealed record ProductLookupResult(ProductLookupOutcome Outcome, bool CanContribute, ProductResponse? Product);

public enum AlternativesOutcome
{
    Found,
    NotFound,
    PremiumRequired
}

public sealed record AlternativesResult(AlternativesOutcome Outcome, IReadOnlyList<ProductResponse> Alternatives);

public interface IProductLookupService
{
    Task<ProductLookupResult> GetByBarcodeAsync(Guid userId, string rawBarcode, CancellationToken ct);
    Task<AlternativesResult> GetAlternativesAsync(Guid userId, string rawBarcode, CancellationToken ct);
}
