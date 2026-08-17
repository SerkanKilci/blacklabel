using Blacklabel.Application.Scoring;
using Xunit;

namespace Blacklabel.Tests.Scoring;

public class ScoreCalculatorTests
{
    private static ScoreCalculator CreateCalculator() => new(new ScoreThresholds());

    [Fact]
    public void Calculate_Returns_Null_Score_When_No_Data_Available()
    {
        var calculator = CreateCalculator();

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: null, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: null, NovaGroup: null, IngredientsTextLength: null));

        Assert.Null(result.Score);
        Assert.Null(result.NutritionScore);
        Assert.Null(result.AdditivesScore);
        Assert.Null(result.ProcessingScore);
    }

    [Fact]
    public void Calculate_Returns_100_For_Best_Possible_Product()
    {
        var calculator = CreateCalculator();

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: 0, SaturatedFat100g: 0, Salt100g: 0, EnergyKcal100g: 0,
            Fiber100g: 10, Protein100g: 20,
            AdditiveRiskLevels: Array.Empty<int>(), NovaGroup: 1, IngredientsTextLength: 20));

        Assert.Equal(50, result.NutritionScore);
        Assert.Equal(35, result.AdditivesScore);
        Assert.Equal(15, result.ProcessingScore);
        Assert.Equal(100, result.Score);
        Assert.False(result.ProcessingEstimated);
    }

    [Fact]
    public void Calculate_Returns_0_For_Worst_Possible_Product()
    {
        var calculator = CreateCalculator();

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: 100, SaturatedFat100g: 50, Salt100g: 10, EnergyKcal100g: 900,
            Fiber100g: 0, Protein100g: 0,
            AdditiveRiskLevels: new[] { 3, 3, 3, 3 }, NovaGroup: 4, IngredientsTextLength: 500));

        Assert.Equal(0, result.NutritionScore);
        Assert.Equal(0, result.AdditivesScore);
        Assert.Equal(0, result.ProcessingScore);
        Assert.Equal(0, result.Score);
    }

    [Fact]
    public void Calculate_Prorates_Total_When_Only_Nutrition_Data_Available()
    {
        var calculator = CreateCalculator();
        var thresholds = new ScoreThresholds();

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: thresholds.Sugars100g.GoodValue, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: null, NovaGroup: null, IngredientsTextLength: null));

        Assert.Equal(50, result.NutritionScore);
        Assert.Null(result.AdditivesScore);
        Assert.Null(result.ProcessingScore);
        Assert.Equal(100, result.Score);
    }

    [Fact]
    public void Calculate_Additives_Score_Deducts_By_Risk_Level_And_Floors_At_Zero()
    {
        var calculator = CreateCalculator();

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: null, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: new[] { 1, 2, 3 }, NovaGroup: null, IngredientsTextLength: null));

        // 35 - 2 (risk1) - 6 (risk2) - 12 (risk3) = 15
        Assert.Equal(15, result.AdditivesScore);
    }

    [Fact]
    public void Calculate_Additives_Score_Never_Goes_Below_Zero()
    {
        var calculator = CreateCalculator();

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: null, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: new[] { 3, 3, 3, 3, 3 }, NovaGroup: null, IngredientsTextLength: null));

        Assert.Equal(0, result.AdditivesScore);
    }

    [Theory]
    [InlineData(1, 15)]
    [InlineData(2, 11)]
    [InlineData(3, 6)]
    [InlineData(4, 0)]
    public void Calculate_Processing_Score_Uses_Nova_Group_Table_When_Known(int novaGroup, int expectedScore)
    {
        var calculator = CreateCalculator();

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: null, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: null, NovaGroup: novaGroup, IngredientsTextLength: null));

        Assert.Equal(expectedScore, result.ProcessingScore);
        Assert.False(result.ProcessingEstimated);
    }

    [Fact]
    public void Calculate_Estimates_Processing_Score_From_Additives_And_Ingredients_Length_When_Nova_Missing()
    {
        var calculator = CreateCalculator();

        var manyAdditivesResult = calculator.Calculate(new ScoreInput(
            Sugars100g: null, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: new[] { 1, 1, 1, 1, 1 }, NovaGroup: null, IngredientsTextLength: 50));

        Assert.Equal(0, manyAdditivesResult.ProcessingScore);
        Assert.True(manyAdditivesResult.ProcessingEstimated);

        var noAdditivesResult = calculator.Calculate(new ScoreInput(
            Sugars100g: null, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: Array.Empty<int>(), NovaGroup: null, IngredientsTextLength: 20));

        Assert.Equal(15, noAdditivesResult.ProcessingScore);
        Assert.True(noAdditivesResult.ProcessingEstimated);
    }

    [Fact]
    public void Calculate_Nutrition_Score_Interpolates_Between_Good_And_Bad_Thresholds()
    {
        var calculator = CreateCalculator();
        var thresholds = new ScoreThresholds();
        var midpointSugar = (thresholds.Sugars100g.GoodValue + thresholds.Sugars100g.BadValue) / 2;

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: midpointSugar, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: null, NovaGroup: null, IngredientsTextLength: null));

        // Midpoint between good and bad should score ~5 raw -> 25 out of 50.
        Assert.Equal(25, result.NutritionScore);
    }

    [Fact]
    public void Calculate_Nutrition_Score_Clamps_Beyond_Bad_Threshold_To_Zero()
    {
        var calculator = CreateCalculator();
        var thresholds = new ScoreThresholds();

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: thresholds.Sugars100g.BadValue + 100, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: null, NovaGroup: null, IngredientsTextLength: null));

        Assert.Equal(0, result.NutritionScore);
    }

    [Fact]
    public void Calculate_Returns_Null_Comparison_Levels_When_No_Data_Available()
    {
        var calculator = CreateCalculator();

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: null, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: null, NovaGroup: null, IngredientsTextLength: null));

        Assert.Null(result.SugarLevel);
        Assert.Null(result.SaturatedFatLevel);
        Assert.Null(result.SaltLevel);
        Assert.Null(result.AdditivesLevel);
    }

    [Theory]
    [InlineData(5.0, ComparisonLevel.Good)] // at goodValue
    [InlineData(0.0, ComparisonLevel.Good)] // below goodValue
    [InlineData(22.5, ComparisonLevel.Bad)] // at badValue
    [InlineData(100.0, ComparisonLevel.Bad)] // beyond badValue
    [InlineData(13.75, ComparisonLevel.Medium)] // midpoint
    public void Calculate_Classifies_Sugar_Level_Using_The_Same_Thresholds_As_The_Score(double sugars, ComparisonLevel expected)
    {
        var calculator = CreateCalculator();

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: (decimal)sugars, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: null, NovaGroup: null, IngredientsTextLength: null));

        Assert.Equal(expected, result.SugarLevel);
    }

    [Theory]
    [InlineData(new int[] { }, ComparisonLevel.Good)] // 0 points lost
    [InlineData(new[] { 1 }, ComparisonLevel.Medium)] // 2 points lost
    [InlineData(new[] { 2 }, ComparisonLevel.Medium)] // 6 points lost (boundary, still Medium)
    [InlineData(new[] { 3 }, ComparisonLevel.Bad)] // 12 points lost
    [InlineData(new[] { 1, 1, 1, 1 }, ComparisonLevel.Bad)] // 8 points lost
    public void Calculate_Classifies_Additives_Level_From_The_Same_Penalty_Points_As_The_Score(
        int[] riskLevels, ComparisonLevel expected)
    {
        var calculator = CreateCalculator();

        var result = calculator.Calculate(new ScoreInput(
            Sugars100g: null, SaturatedFat100g: null, Salt100g: null, EnergyKcal100g: null,
            Fiber100g: null, Protein100g: null,
            AdditiveRiskLevels: riskLevels, NovaGroup: null, IngredientsTextLength: null));

        Assert.Equal(expected, result.AdditivesLevel);
    }
}
