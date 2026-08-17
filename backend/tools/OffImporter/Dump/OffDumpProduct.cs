using System.Text.Json;
using System.Text.Json.Serialization;

namespace OffImporter.Dump;

/// <summary>
/// Raw shape of a single line in the Open Food Facts JSONL bulk export. Mirrors the fields
/// requested from the live product API (see OpenFoodFactsClient.Fields) plus the two fields
/// only needed for the ETL's own filtering: "code" (barcode) and "countries_tags".
/// </summary>
public sealed class OffDumpProduct
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("product_name")]
    public string? ProductName { get; set; }

    [JsonPropertyName("product_name_tr")]
    public string? ProductNameTr { get; set; }

    [JsonPropertyName("brands")]
    public string? Brands { get; set; }

    [JsonPropertyName("quantity")]
    public string? Quantity { get; set; }

    [JsonPropertyName("ingredients_text")]
    public string? IngredientsText { get; set; }

    [JsonPropertyName("ingredients_text_tr")]
    public string? IngredientsTextTr { get; set; }

    [JsonPropertyName("additives_tags")]
    public List<string>? AdditivesTags { get; set; }

    [JsonPropertyName("allergens_tags")]
    public List<string>? AllergensTags { get; set; }

    [JsonPropertyName("nova_group")]
    public int? NovaGroup { get; set; }

    [JsonPropertyName("nutriscore_grade")]
    public string? NutriscoreGrade { get; set; }

    [JsonPropertyName("nutriments")]
    public Dictionary<string, JsonElement>? Nutriments { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("categories_tags")]
    public List<string>? CategoriesTags { get; set; }

    [JsonPropertyName("countries_tags")]
    public List<string>? CountriesTags { get; set; }
}
