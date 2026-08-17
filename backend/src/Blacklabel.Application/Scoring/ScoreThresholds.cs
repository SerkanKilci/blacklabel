namespace Blacklabel.Application.Scoring;

public sealed record NutrientBand(decimal GoodValue, decimal BadValue);

public sealed class ScoreThresholds
{
    public NutrientBand Sugars100g { get; set; } = new(GoodValue: 5m, BadValue: 22.5m);
    public NutrientBand SaturatedFat100g { get; set; } = new(GoodValue: 1m, BadValue: 10m);
    public NutrientBand Salt100g { get; set; } = new(GoodValue: 0.3m, BadValue: 2.0m);
    public NutrientBand EnergyKcal100g { get; set; } = new(GoodValue: 80m, BadValue: 400m);
    public NutrientBand Fiber100g { get; set; } = new(GoodValue: 4.7m, BadValue: 0m);
    public NutrientBand Protein100g { get; set; } = new(GoodValue: 8m, BadValue: 0m);
}
