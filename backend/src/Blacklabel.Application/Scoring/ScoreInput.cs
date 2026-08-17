namespace Blacklabel.Application.Scoring;

public sealed record ScoreInput(
    decimal? Sugars100g,
    decimal? SaturatedFat100g,
    decimal? Salt100g,
    decimal? EnergyKcal100g,
    decimal? Fiber100g,
    decimal? Protein100g,
    IReadOnlyList<int>? AdditiveRiskLevels,
    int? NovaGroup,
    int? IngredientsTextLength
);
