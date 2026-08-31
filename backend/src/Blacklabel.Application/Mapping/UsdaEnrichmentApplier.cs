using System.Text.Json;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.ExternalModels;
using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Mapping;

/// <summary>
/// Fills gaps a barcode-confirmed Open Food Facts product left empty using a USDA FoodData
/// Central match. Only ever fills fields OFF left null/blank -- OFF's own data always wins where
/// present.
///
/// Deliberately does NOT touch DataQuality. USDA gives us free-text ingredients and nutrients but
/// no structured allergen signal equivalent to OFF's community-tagged allergens_tags, so a
/// product enriched this way still can't back the "no allergens flagged" claim that
/// DataQuality.Complete makes to users -- it stays whatever OFF determined (always Partial when
/// this runs, since enrichment only triggers on a Partial product), and the incomplete-data
/// warning keeps showing.
/// </summary>
public static class UsdaEnrichmentApplier
{
    public static void Apply(Product product, UsdaFoodItem usda)
    {
        if (string.IsNullOrWhiteSpace(product.IngredientsText) && !string.IsNullOrWhiteSpace(usda.IngredientsText))
        {
            product.IngredientsText = usda.IngredientsText;
        }

        product.Nutriments = JsonSerializer.Serialize(MergeNutriments(product.Nutriments, usda.Nutriments));
        product.UsdaEnrichedAt = DateTime.UtcNow;
    }

    private static NutrimentsDto MergeNutriments(string? existingJson, NutrimentsDto usda)
    {
        var existing = string.IsNullOrWhiteSpace(existingJson)
            ? null
            : JsonSerializer.Deserialize<NutrimentsDto>(existingJson);

        return new NutrimentsDto(
            existing?.EnergyKcal100g ?? usda.EnergyKcal100g,
            existing?.Fat100g ?? usda.Fat100g,
            existing?.SaturatedFat100g ?? usda.SaturatedFat100g,
            existing?.Carbohydrates100g ?? usda.Carbohydrates100g,
            existing?.Sugars100g ?? usda.Sugars100g,
            existing?.Fiber100g ?? usda.Fiber100g,
            existing?.Proteins100g ?? usda.Proteins100g,
            existing?.Salt100g ?? usda.Salt100g);
    }
}
