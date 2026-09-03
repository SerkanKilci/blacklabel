using System.Text.Json;
using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;
using Blacklabel.Application.Matching;
using Blacklabel.Application.Scoring;
using Blacklabel.Domain.Entities;
using Blacklabel.Domain.Enums;

namespace Blacklabel.Application.Mapping;

/// <summary>
/// Applies an Open Food Facts product payload onto a Domain <see cref="Product"/>, matching
/// additives/allergens against the seeded reference tables and computing the score. Shared by
/// the live lookup path (ProductLookupService) and the offline ETL importer (tools/OffImporter)
/// so both stay in sync on exactly how OFF data becomes a stored Product.
/// </summary>
public static class ProductFromOffMapper
{
    public static async Task<IReadOnlyList<Additive>> ApplyAsync(
        Product product,
        OpenFoodFactsProduct off,
        IAdditiveRepository additiveRepository,
        IAllergenRepository allergenRepository,
        ScoreCalculator scoreCalculator,
        CancellationToken ct)
    {
        var matchedAdditives = new List<Additive>();
        foreach (var code in off.AdditivesTags.Distinct())
        {
            var additive = await additiveRepository.GetByCodeAsync(code, ct);
            if (additive is not null)
            {
                matchedAdditives.Add(additive);
            }
        }

        var matchedAllergenCodes = new List<string>();
        foreach (var code in off.AllergensTags.Distinct())
        {
            var allergen = await allergenRepository.GetByCodeAsync(code, ct);
            if (allergen is not null)
            {
                matchedAllergenCodes.Add(allergen.Code);
            }
        }

        var scoreInput = new ScoreInput(
            off.Nutriments.Sugars100g,
            off.Nutriments.SaturatedFat100g,
            off.Nutriments.Salt100g,
            off.Nutriments.EnergyKcal100g,
            off.Nutriments.Fiber100g,
            off.Nutriments.Proteins100g,
            matchedAdditives.Select(a => a.RiskLevel).ToList(),
            off.NovaGroup,
            off.IngredientsText?.Length);

        var scoreResult = scoreCalculator.Calculate(scoreInput);
        var now = DateTime.UtcNow;

        var rawName = off.ProductNameTr ?? off.ProductName ?? string.Empty;
        product.Name = rawName.Length <= 300 ? rawName : rawName[..300];
        product.Brand = Truncate(off.Brands, 200);
        product.Quantity = Truncate(off.Quantity, 100);
        product.IngredientsText = off.IngredientsText;
        product.IngredientsTextTr = off.IngredientsTextTr;
        product.NovaGroup = off.NovaGroup;
        product.NutriScore = Truncate(off.NutriscoreGrade?.ToLowerInvariant(), 1);
        product.Nutriments = JsonSerializer.Serialize(off.Nutriments);
        product.ImageUrl = Truncate(off.ImageUrl, 500);
        product.Categories = JsonSerializer.Serialize(off.CategoriesTags);
        product.Source = ProductSource.OpenFoodFacts;
        product.DataQuality = DetermineDataQuality(off, matchedAllergenCodes);
        product.Score = scoreResult.Score;
        product.ScoreCalculatedAt = now;
        product.UpdatedAt = now;

        product.ProductAdditives.Clear();
        foreach (var additive in matchedAdditives)
        {
            product.ProductAdditives.Add(new ProductAdditive { ProductId = product.Id, AdditiveCode = additive.Code });
        }

        product.ProductAllergens.Clear();
        foreach (var code in matchedAllergenCodes)
        {
            product.ProductAllergens.Add(new ProductAllergen { ProductId = product.Id, AllergenCode = code });
        }

        return matchedAdditives;
    }

    // Complete depends only on ingredients being present, not on how many nutriment fields OFF's
    // contributors happened to fill in. This flag drives the mobile "data incomplete" safety
    // warning, which is about allergen/additive trustworthiness -- that's a function of whether we
    // have an ingredients list, not of nutrition-facts completeness (which only affects score
    // precision and is handled independently via null-tolerant ScoreInput fields).
    private static DataQuality DetermineDataQuality(OpenFoodFactsProduct off, IReadOnlyList<string> matchedAllergenCodes)
    {
        var hasIngredients = !string.IsNullOrWhiteSpace(off.IngredientsText) || !string.IsNullOrWhiteSpace(off.IngredientsTextTr);
        if (!hasIngredients)
        {
            return DataQuality.Partial;
        }

        // Safety net: OFF's allergens_tags came back empty, but the ingredients text itself
        // mentions a likely allergen. Don't claim Complete when we can't back the "nothing
        // flagged" implication that carries for the user -- see AllergenKeywordScanner.
        var possiblyUnflaggedAllergen = matchedAllergenCodes.Count == 0 &&
            (AllergenKeywordScanner.MentionsPossibleAllergen(off.IngredientsText) ||
             AllergenKeywordScanner.MentionsPossibleAllergen(off.IngredientsTextTr));

        return possiblyUnflaggedAllergen ? DataQuality.Partial : DataQuality.Complete;
    }

    private static string? Truncate(string? value, int maxLength)
        => string.IsNullOrEmpty(value) ? value : (value.Length <= maxLength ? value : value[..maxLength]);
}
