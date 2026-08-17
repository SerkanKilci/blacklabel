using System.Globalization;
using System.Text.Json;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.ExternalModels;

namespace OffImporter.Dump;

/// <summary>
/// Converts a raw dump line into the same <see cref="OpenFoodFactsProduct"/> shape the live API
/// client produces, so both paths feed identical data into ProductFromOffMapper.
/// </summary>
public static class OffDumpMapper
{
    public static OpenFoodFactsProduct ToOpenFoodFactsProduct(OffDumpProduct raw)
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
