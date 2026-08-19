using Blacklabel.Application.Dtos;

namespace Blacklabel.Application.Interfaces;

public enum ProductLookupOutcome
{
    Found,
    NotFound,
    InvalidBarcode,
    DailyLimitExceeded,
    /// <summary>
    /// Not in our cache, and we couldn't reach Open Food Facts (or held back to respect our own
    /// rate limit toward them) to check. Distinct from NotFound: the product may well exist.
    /// </summary>
    LookupUnavailable
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
