namespace Blacklabel.Application.Scoring;

public sealed record ScoreResult(
    int? Score,
    int? NutritionScore,
    int? AdditivesScore,
    int? ProcessingScore,
    bool ProcessingEstimated,
    ComparisonLevel? SugarLevel,
    ComparisonLevel? SaturatedFatLevel,
    ComparisonLevel? SaltLevel,
    ComparisonLevel? AdditivesLevel
);
