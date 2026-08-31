using System.Text.Json;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Mapping;
using Blacklabel.Domain.Entities;
using Xunit;

namespace Blacklabel.Tests.Mapping;

public class UsdaEnrichmentApplierTests
{
    private static readonly UsdaFoodItem UsdaMatch = new(
        GtinUpc: "00016000275287",
        IngredientsText: "USDA ingredients",
        Nutriments: new NutrimentsDto(
            EnergyKcal100g: 100, Fat100g: 10, SaturatedFat100g: 5, Carbohydrates100g: 20,
            Sugars100g: 8, Fiber100g: 3, Proteins100g: 4, Salt100g: 0.5m));

    [Fact]
    public void Apply_Fills_IngredientsText_When_Off_Left_It_Empty()
    {
        var product = new Product { IngredientsText = null };

        UsdaEnrichmentApplier.Apply(product, UsdaMatch);

        Assert.Equal("USDA ingredients", product.IngredientsText);
    }

    [Fact]
    public void Apply_Never_Overwrites_Ingredients_Off_Already_Provided()
    {
        var product = new Product { IngredientsText = "OFF ingredients" };

        UsdaEnrichmentApplier.Apply(product, UsdaMatch);

        Assert.Equal("OFF ingredients", product.IngredientsText);
    }

    [Fact]
    public void Apply_Only_Fills_Nutrient_Fields_Off_Left_Null_And_Keeps_The_Rest()
    {
        var offNutriments = new NutrimentsDto(
            EnergyKcal100g: 200, Fat100g: null, SaturatedFat100g: null, Carbohydrates100g: null,
            Sugars100g: null, Fiber100g: null, Proteins100g: null, Salt100g: null);
        var product = new Product { Nutriments = JsonSerializer.Serialize(offNutriments) };

        UsdaEnrichmentApplier.Apply(product, UsdaMatch);

        var merged = JsonSerializer.Deserialize<NutrimentsDto>(product.Nutriments!)!;
        Assert.Equal(200, merged.EnergyKcal100g); // OFF's value wins
        Assert.Equal(10, merged.Fat100g);         // USDA fills the gap
        Assert.Equal(0.5m, merged.Salt100g);      // USDA fills the gap
    }

    [Fact]
    public void Apply_Sets_UsdaEnrichedAt_So_The_Product_Is_Not_Requeried()
    {
        var product = new Product();

        UsdaEnrichmentApplier.Apply(product, UsdaMatch);

        Assert.NotNull(product.UsdaEnrichedAt);
    }

    [Fact]
    public void Apply_Never_Changes_DataQuality()
    {
        var product = new Product { DataQuality = Blacklabel.Domain.Enums.DataQuality.Partial };

        UsdaEnrichmentApplier.Apply(product, UsdaMatch);

        // Deliberate: USDA has no structured allergen signal, so enrichment must never promote a
        // product to Complete and silence the incomplete-data safety warning.
        Assert.Equal(Blacklabel.Domain.Enums.DataQuality.Partial, product.DataQuality);
    }
}
