using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Blacklabel.Infrastructure.ExternalClients;

public class OpenFoodFactsClient : IOpenFoodFactsClient
{
    private const string Fields =
        "product_name,product_name_tr,brands,quantity,ingredients_text,ingredients_text_tr,additives_tags,allergens_tags,nova_group,nutriscore_grade,nutriments,image_url,categories_tags";

    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenFoodFactsClient> _logger;

    public OpenFoodFactsClient(HttpClient httpClient, ILogger<OpenFoodFactsClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<OpenFoodFactsProduct?> GetProductAsync(string barcode, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{barcode}.json?fields={Fields}", ct);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Open Food Facts request failed for {Barcode} with status {StatusCode}", barcode, response.StatusCode);
                return null;
            }

            var payload = await response.Content.ReadFromJsonAsync<OpenFoodFactsEnvelope>(cancellationToken: ct);
            if (payload is null || payload.Status == 0 || payload.Product is null)
            {
                return null;
            }

            return MapToProduct(payload.Product);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            _logger.LogWarning(ex, "Open Food Facts lookup failed for {Barcode}", barcode);
            return null;
        }
    }

    private static OpenFoodFactsProduct MapToProduct(OpenFoodFactsRawProduct raw)
    {
        var nutrimentsRaw = raw.Nutriments ?? new Dictionary<string, JsonElement>();
        var nutriments = new NutrimentsDto(
            GetDecimal(nutrimentsRaw, "energy-kcal_100g"),
            GetDecimal(nutrimentsRaw, "fat_100g"),
            GetDecimal(nutrimentsRaw, "saturated-fat_100g"),
            GetDecimal(nutrimentsRaw, "carbohydrates_100g"),
            GetDecimal(nutrimentsRaw, "sugars_100g"),
            GetDecimal(nutrimentsRaw, "fiber_100g"),
            GetDecimal(nutrimentsRaw, "proteins_100g"),
            GetDecimal(nutrimentsRaw, "salt_100g"));

        return new OpenFoodFactsProduct(
            raw.ProductName,
            raw.ProductNameTr,
            raw.Brands,
            raw.Quantity,
            raw.IngredientsText,
            raw.IngredientsTextTr,
            (raw.AdditivesTags ?? new List<string>()).Select(OpenFoodFactsTagCleaner.CleanAdditiveTag).ToList(),
            (raw.AllergensTags ?? new List<string>()).Select(OpenFoodFactsTagCleaner.CleanAllergenTag).ToList(),
            raw.NovaGroup,
            raw.NutriscoreGrade,
            nutriments,
            raw.ImageUrl,
            (raw.CategoriesTags ?? new List<string>()).Select(OpenFoodFactsTagCleaner.CleanCategoryTag).ToList());
    }

    private static decimal? GetDecimal(Dictionary<string, JsonElement> nutriments, string key)
    {
        if (!nutriments.TryGetValue(key, out var element))
        {
            return null;
        }

        return element.ValueKind switch
        {
            JsonValueKind.Number when element.TryGetDecimal(out var value) => value,
            JsonValueKind.String when decimal.TryParse(element.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed) => parsed,
            _ => null
        };
    }
}
