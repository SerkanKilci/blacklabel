namespace Blacklabel.Application.Scoring;

public sealed class ScoreCalculator
{
    private const int NutritionMaxPoints = 50;
    private const int AdditivesMaxPoints = 35;
    private const int ProcessingMaxPoints = 15;

    private readonly ScoreThresholds _thresholds;

    public ScoreCalculator(ScoreThresholds thresholds)
    {
        _thresholds = thresholds;
    }

    public ScoreResult Calculate(ScoreInput input)
    {
        var nutritionScore = CalculateNutritionScore(input);
        var additivesScore = CalculateAdditivesScore(input.AdditiveRiskLevels);
        var (processingScore, processingEstimated) = CalculateProcessingScore(input);

        var achievedPoints = 0m;
        var maxPoints = 0m;

        if (nutritionScore is not null)
        {
            achievedPoints += nutritionScore.Value;
            maxPoints += NutritionMaxPoints;
        }

        if (additivesScore is not null)
        {
            achievedPoints += additivesScore.Value;
            maxPoints += AdditivesMaxPoints;
        }

        if (processingScore is not null)
        {
            achievedPoints += processingScore.Value;
            maxPoints += ProcessingMaxPoints;
        }

        int? totalScore = maxPoints == 0
            ? null
            : (int)Math.Round(achievedPoints / maxPoints * 100, MidpointRounding.AwayFromZero);

        var sugarLevel = input.Sugars100g is { } sugars ? ClassifyNegative(sugars, _thresholds.Sugars100g) : (ComparisonLevel?)null;
        var saturatedFatLevel = input.SaturatedFat100g is { } saturatedFat
            ? ClassifyNegative(saturatedFat, _thresholds.SaturatedFat100g)
            : (ComparisonLevel?)null;
        var saltLevel = input.Salt100g is { } salt ? ClassifyNegative(salt, _thresholds.Salt100g) : (ComparisonLevel?)null;
        var additivesLevel = input.AdditiveRiskLevels is { } riskLevels ? ClassifyAdditives(riskLevels) : (ComparisonLevel?)null;

        return new ScoreResult(
            totalScore, nutritionScore, additivesScore, processingScore, processingEstimated,
            sugarLevel, saturatedFatLevel, saltLevel, additivesLevel);
    }

    private int? CalculateNutritionScore(ScoreInput input)
    {
        var rawScores = new List<decimal>();

        if (input.Sugars100g is { } sugars)
        {
            rawScores.Add(RawScoreForNegative(sugars, _thresholds.Sugars100g));
        }

        if (input.SaturatedFat100g is { } saturatedFat)
        {
            rawScores.Add(RawScoreForNegative(saturatedFat, _thresholds.SaturatedFat100g));
        }

        if (input.Salt100g is { } salt)
        {
            rawScores.Add(RawScoreForNegative(salt, _thresholds.Salt100g));
        }

        if (input.EnergyKcal100g is { } energy)
        {
            rawScores.Add(RawScoreForNegative(energy, _thresholds.EnergyKcal100g));
        }

        if (input.Fiber100g is { } fiber)
        {
            rawScores.Add(RawScoreForPositive(fiber, _thresholds.Fiber100g));
        }

        if (input.Protein100g is { } protein)
        {
            rawScores.Add(RawScoreForPositive(protein, _thresholds.Protein100g));
        }

        if (rawScores.Count == 0)
        {
            return null;
        }

        var averageRaw = rawScores.Average();
        return (int)Math.Round(averageRaw * (NutritionMaxPoints / 10m), MidpointRounding.AwayFromZero);
    }

    private static int? CalculateAdditivesScore(IReadOnlyList<int>? additiveRiskLevels)
    {
        if (additiveRiskLevels is null)
        {
            return null;
        }

        var pointsLost = additiveRiskLevels.Sum(AdditivePenaltyPoints);
        return Math.Max(0, AdditivesMaxPoints - pointsLost);
    }

    // Shared by CalculateAdditivesScore and ClassifyAdditives so the "Good/Medium/Bad" label
    // shown in product comparisons can never disagree with the points actually deducted.
    private static int AdditivePenaltyPoints(int riskLevel) => riskLevel switch
    {
        1 => 2,
        2 => 6,
        3 => 12,
        _ => 0
    };

    private static (int? Score, bool Estimated) CalculateProcessingScore(ScoreInput input)
    {
        if (input.NovaGroup is { } novaGroup)
        {
            var score = novaGroup switch
            {
                1 => ProcessingMaxPoints,
                2 => 11,
                3 => 6,
                4 => 0,
                _ => (int?)null
            };

            return (score, false);
        }

        if (input.IngredientsTextLength is { } ingredientsLength)
        {
            var additiveCount = input.AdditiveRiskLevels?.Count ?? 0;
            var estimatedNova = EstimateNovaGroup(additiveCount, ingredientsLength);
            var score = estimatedNova switch
            {
                1 => ProcessingMaxPoints,
                2 => 11,
                3 => 6,
                _ => 0
            };

            return (score, true);
        }

        return (null, false);
    }

    private static int EstimateNovaGroup(int additiveCount, int ingredientsTextLength)
    {
        if (additiveCount >= 5 || ingredientsTextLength > 300)
        {
            return 4;
        }

        if (additiveCount >= 2)
        {
            return 3;
        }

        if (additiveCount == 1)
        {
            return 2;
        }

        return 1;
    }

    private static decimal RawScoreForNegative(decimal value, NutrientBand band)
    {
        if (value <= band.GoodValue)
        {
            return 10;
        }

        if (value >= band.BadValue)
        {
            return 0;
        }

        var ratio = (band.BadValue - value) / (band.BadValue - band.GoodValue);
        return 10 * ratio;
    }

    private static decimal RawScoreForPositive(decimal value, NutrientBand band)
    {
        if (value >= band.GoodValue)
        {
            return 10;
        }

        if (value <= band.BadValue)
        {
            return 0;
        }

        var ratio = (value - band.BadValue) / (band.GoodValue - band.BadValue);
        return 10 * ratio;
    }

    // Same boundary comparisons as RawScoreForNegative, just returning a 3-band label instead of
    // a 0-10 raw point value — kept as separate methods rather than deriving one from the other
    // so neither has to guess at the other's rounding.
    private static ComparisonLevel ClassifyNegative(decimal value, NutrientBand band)
    {
        if (value <= band.GoodValue)
        {
            return ComparisonLevel.Good;
        }

        if (value >= band.BadValue)
        {
            return ComparisonLevel.Bad;
        }

        return ComparisonLevel.Medium;
    }

    private static ComparisonLevel ClassifyAdditives(IReadOnlyList<int> additiveRiskLevels)
    {
        var pointsLost = additiveRiskLevels.Sum(AdditivePenaltyPoints);

        if (pointsLost == 0)
        {
            return ComparisonLevel.Good;
        }

        if (pointsLost <= 6)
        {
            return ComparisonLevel.Medium;
        }

        return ComparisonLevel.Bad;
    }
}
