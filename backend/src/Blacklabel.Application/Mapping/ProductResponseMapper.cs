using System.Text.Json;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.Scoring;
using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Mapping;

public static class ProductResponseMapper
{
    public static ProductResponse ToResponse(Product product, IReadOnlyList<Additive> matchedAdditives, ScoreResult scoreResult)
    {
        var nutriments = DeserializeNutriments(product.Nutriments);

        var additives = matchedAdditives
            .Select(AdditiveMapper.ToResponse)
            .OrderBy(a => a.Code)
            .ToList();

        var allergens = product.ProductAllergens
            .Select(pa => pa.AllergenCode)
            .OrderBy(code => code)
            .ToList();

        var breakdown = scoreResult.Score is null
            ? null
            : new ScoreBreakdownDto(scoreResult.NutritionScore, scoreResult.AdditivesScore, scoreResult.ProcessingScore);

        var comparisonBands = new ComparisonBands(
            scoreResult.SugarLevel?.ToString(),
            scoreResult.SaturatedFatLevel?.ToString(),
            scoreResult.SaltLevel?.ToString(),
            scoreResult.AdditivesLevel?.ToString());

        return new ProductResponse(
            Found: true,
            Barcode: product.Barcode,
            Name: product.Name,
            Brand: product.Brand,
            ImageUrl: product.ImageUrl,
            Score: scoreResult.Score,
            ScoreBreakdown: breakdown,
            NovaGroup: product.NovaGroup,
            NutriScore: product.NutriScore,
            IngredientsText: product.IngredientsTextTr ?? product.IngredientsText,
            Additives: additives,
            Allergens: allergens,
            Nutriments: nutriments,
            ProfileWarnings: Array.Empty<ProfileWarningDto>(),
            DataQuality: product.DataQuality.ToString(),
            Source: product.Source.ToString(),
            ComparisonBands: comparisonBands);
    }

    public static NutrimentsDto? DeserializeNutriments(string? json)
        => string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<NutrimentsDto>(json);
}
