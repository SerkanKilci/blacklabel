using System.Net.Http.Json;
using System.Text.Json;
using Blacklabel.Application.Barcode;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Blacklabel.Infrastructure.ExternalClients;

public class UsdaFoodDataClient : IUsdaFoodDataClient
{
    // FDC's standard nutrient numbers (stable across their dataset, unlike nutrientId which can
    // vary by data source). Sodium is reported, not salt -- salt_g = sodium_mg * 2.5 / 1000 is the
    // conventional conversion factor (salt = NaCl, sodium is ~39% of its mass by molar ratio).
    private const string NutrientEnergyKcal = "208";
    private const string NutrientFat = "204";
    private const string NutrientSaturatedFat = "606";
    private const string NutrientCarbohydrates = "205";
    private const string NutrientSugars = "269";
    private const string NutrientFiber = "291";
    private const string NutrientProtein = "203";
    private const string NutrientSodium = "307";

    private readonly HttpClient _httpClient;
    private readonly UsdaRateLimiter _rateLimiter;
    private readonly string _apiKey;
    private readonly ILogger<UsdaFoodDataClient> _logger;

    // Takes the wrapper record (not a bare string) so the typed-client DI registration
    // (AddHttpClient<IUsdaFoodDataClient, UsdaFoodDataClient>) can resolve it from the container --
    // a raw `string` constructor parameter is ambiguous for the DI container to resolve.
    public UsdaFoodDataClient(HttpClient httpClient, UsdaRateLimiter rateLimiter, UsdaApiKey apiKey, ILogger<UsdaFoodDataClient> logger)
    {
        _httpClient = httpClient;
        _rateLimiter = rateLimiter;
        _apiKey = apiKey.Value;
        _logger = logger;
    }

    public async Task<UsdaLookupResult> GetProductAsync(string barcode, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            return new UsdaLookupResult(UsdaLookupOutcome.Unavailable, null);
        }

        if (!await _rateLimiter.WaitForPermitAsync(ct))
        {
            _logger.LogWarning("USDA FoodData Central self-throttle exhausted, skipping lookup for {Barcode}", barcode);
            return new UsdaLookupResult(UsdaLookupOutcome.Unavailable, null);
        }

        try
        {
            var query = Uri.EscapeDataString(barcode);
            var apiKey = Uri.EscapeDataString(_apiKey);
            var url = $"foods/search?query={query}&dataType=Branded&pageSize=25&api_key={apiKey}";
            var response = await _httpClient.GetAsync(url, ct);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("USDA FoodData Central request failed for {Barcode} with status {StatusCode}", barcode, response.StatusCode);
                return new UsdaLookupResult(UsdaLookupOutcome.Unavailable, null);
            }

            var payload = await response.Content.ReadFromJsonAsync<UsdaSearchResponse>(cancellationToken: ct);
            var candidates = payload?.Foods ?? new List<UsdaRawFood>();

            // FDC's barcode "search" is a fuzzy full-text query, not a lookup -- it can return
            // products that have nothing to do with the barcode searched for. Only a candidate
            // whose own gtinUpc field matches (after GS1 zero-padding normalization) is trusted.
            // Zero or more-than-one confirmed match is treated the same as "not found": we would
            // rather show nothing than guess which of several matches is correct.
            var confirmed = candidates.Where(f => UsdaBarcodeMatcher.IsMatch(barcode, f.GtinUpc)).ToList();
            if (confirmed.Count != 1)
            {
                return new UsdaLookupResult(UsdaLookupOutcome.NotFound, null);
            }

            return new UsdaLookupResult(UsdaLookupOutcome.Found, MapToFoodItem(confirmed[0]));
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            _logger.LogWarning(ex, "USDA FoodData Central lookup failed for {Barcode}", barcode);
            return new UsdaLookupResult(UsdaLookupOutcome.Unavailable, null);
        }
    }

    private static UsdaFoodItem MapToFoodItem(UsdaRawFood raw)
    {
        var nutrients = (raw.FoodNutrients ?? new List<UsdaRawNutrient>())
            .Where(n => n.NutrientNumber is not null)
            .ToDictionary(n => n.NutrientNumber!, n => n.Value);

        var sodiumMg = GetValue(nutrients, NutrientSodium);

        var nutriments = new NutrimentsDto(
            EnergyKcal100g: GetValue(nutrients, NutrientEnergyKcal),
            Fat100g: GetValue(nutrients, NutrientFat),
            SaturatedFat100g: GetValue(nutrients, NutrientSaturatedFat),
            Carbohydrates100g: GetValue(nutrients, NutrientCarbohydrates),
            Sugars100g: GetValue(nutrients, NutrientSugars),
            Fiber100g: GetValue(nutrients, NutrientFiber),
            Proteins100g: GetValue(nutrients, NutrientProtein),
            Salt100g: sodiumMg.HasValue ? sodiumMg.Value * 2.5m / 1000m : null);

        return new UsdaFoodItem(raw.GtinUpc ?? string.Empty, raw.Ingredients, nutriments);
    }

    private static decimal? GetValue(Dictionary<string, decimal?> nutrients, string nutrientNumber)
        => nutrients.TryGetValue(nutrientNumber, out var value) ? value : null;
}
