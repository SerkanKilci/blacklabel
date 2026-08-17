using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blacklabel.Infrastructure.ExternalClients;

internal sealed class OpenFoodFactsEnvelope
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("product")]
    public OpenFoodFactsRawProduct? Product { get; set; }
}

internal sealed class OpenFoodFactsRawProduct
{
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
}
