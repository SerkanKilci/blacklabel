namespace Blacklabel.Application.Dtos;

/// <summary>
/// "Good"/"Medium"/"Bad"/null per category, derived from the same ScoreThresholds/additive-risk
/// values used to compute the product's actual score (see ScoreCalculator) — used by the mobile
/// Compare screen to explain *why* one product outscores another, not just that it does.
/// </summary>
public sealed record ComparisonBands(
    string? Sugar,
    string? SaturatedFat,
    string? Salt,
    string? Additives
);
