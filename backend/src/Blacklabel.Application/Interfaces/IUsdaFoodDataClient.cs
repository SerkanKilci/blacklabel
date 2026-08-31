using Blacklabel.Application.ExternalModels;

namespace Blacklabel.Application.Interfaces;

public interface IUsdaFoodDataClient
{
    Task<UsdaLookupResult> GetProductAsync(string barcode, CancellationToken ct);
}

public enum UsdaLookupOutcome
{
    /// <summary>Exactly one search candidate's gtinUpc matched our barcode -- trusted.</summary>
    Found,
    /// <summary>No candidate's gtinUpc matched (including "no candidates at all").</summary>
    NotFound,
    /// <summary>
    /// Rate-limited, a transient HTTP error, a timeout, or our own self-throttle. Must never be
    /// treated as NotFound -- same reasoning as <see cref="OpenFoodFactsLookupOutcome.Unavailable"/>.
    /// </summary>
    Unavailable
}

public sealed record UsdaLookupResult(UsdaLookupOutcome Outcome, UsdaFoodItem? Product);
