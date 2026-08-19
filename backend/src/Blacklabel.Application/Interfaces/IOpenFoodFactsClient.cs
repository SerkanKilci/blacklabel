using Blacklabel.Application.ExternalModels;

namespace Blacklabel.Application.Interfaces;

public interface IOpenFoodFactsClient
{
    Task<OpenFoodFactsLookupResult> GetProductAsync(string barcode, CancellationToken ct);
}

public enum OpenFoodFactsLookupOutcome
{
    Found,
    /// <summary>Open Food Facts responded normally and confirmed this barcode has no product.</summary>
    NotFound,
    /// <summary>
    /// We couldn't determine whether the product exists — rate-limited, a transient HTTP error,
    /// a timeout, or our own self-throttle. Must never be treated as NotFound: that would tell a
    /// user a real product doesn't exist just because we (or Open Food Facts) were briefly
    /// overloaded.
    /// </summary>
    Unavailable
}

public sealed record OpenFoodFactsLookupResult(OpenFoodFactsLookupOutcome Outcome, OpenFoodFactsProduct? Product);
