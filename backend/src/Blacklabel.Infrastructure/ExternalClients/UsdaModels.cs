using System.Text.Json.Serialization;

namespace Blacklabel.Infrastructure.ExternalClients;

/// <summary>Wraps the raw api_key string so it's a distinct DI-resolvable type (a bare <see cref="string"/> constructor parameter can't be resolved by the container).</summary>
public sealed record UsdaApiKey(string Value);

internal sealed class UsdaSearchResponse
{
    [JsonPropertyName("foods")]
    public List<UsdaRawFood>? Foods { get; set; }
}

internal sealed class UsdaRawFood
{
    [JsonPropertyName("gtinUpc")]
    public string? GtinUpc { get; set; }

    [JsonPropertyName("ingredients")]
    public string? Ingredients { get; set; }

    [JsonPropertyName("foodNutrients")]
    public List<UsdaRawNutrient>? FoodNutrients { get; set; }
}

internal sealed class UsdaRawNutrient
{
    [JsonPropertyName("nutrientNumber")]
    public string? NutrientNumber { get; set; }

    [JsonPropertyName("value")]
    public decimal? Value { get; set; }
}
