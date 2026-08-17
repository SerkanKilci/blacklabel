using Blacklabel.Application.Dtos;
using Blacklabel.Application.Preferences;
using Xunit;

namespace Blacklabel.Tests.Preferences;

public class PersonalWarningCalculatorTests
{
    private static UserPreferenceResponse CreatePreference(
        IReadOnlyList<string>? avoidedAdditiveCodes = null,
        IReadOnlyList<string>? allergenCodes = null,
        DietFlagsDto? dietFlags = null) => new(
            avoidedAdditiveCodes ?? Array.Empty<string>(),
            allergenCodes ?? Array.Empty<string>(),
            dietFlags ?? DietFlagsDto.Empty);

    [Fact]
    public void Calculate_Returns_Empty_When_No_Preference_Set()
    {
        var result = PersonalWarningCalculator.Calculate(null, new[] { "E211" }, new[] { "gluten" }, null);

        Assert.Empty(result);
    }

    [Fact]
    public void Calculate_Warns_On_Matching_Allergen()
    {
        var preference = CreatePreference(allergenCodes: new[] { "milk" });

        var result = PersonalWarningCalculator.Calculate(preference, Array.Empty<string>(), new[] { "milk", "gluten" }, null);

        var warning = Assert.Single(result);
        Assert.Equal("allergen", warning.Type);
        Assert.Equal("milk", warning.Code);
    }

    [Fact]
    public void Calculate_Does_Not_Warn_On_Unlisted_Allergen()
    {
        var preference = CreatePreference(allergenCodes: new[] { "peanuts" });

        var result = PersonalWarningCalculator.Calculate(preference, Array.Empty<string>(), new[] { "milk", "gluten" }, null);

        Assert.Empty(result);
    }

    [Fact]
    public void Calculate_Warns_On_Matching_Avoided_Additive()
    {
        var preference = CreatePreference(avoidedAdditiveCodes: new[] { "E211" });

        var result = PersonalWarningCalculator.Calculate(preference, new[] { "E211", "E330" }, Array.Empty<string>(), null);

        var warning = Assert.Single(result);
        Assert.Equal("additive", warning.Type);
        Assert.Equal("E211", warning.Code);
    }

    [Fact]
    public void Calculate_Warns_On_GlutenFree_Flag_When_Gluten_Present()
    {
        var preference = CreatePreference(dietFlags: DietFlagsDto.Empty with { GlutenFree = true });

        var result = PersonalWarningCalculator.Calculate(preference, Array.Empty<string>(), new[] { "gluten" }, null);

        var warning = Assert.Single(result);
        Assert.Equal("diet", warning.Type);
        Assert.Equal("glutenFree", warning.Code);
    }

    [Fact]
    public void Calculate_Warns_On_LactoseFree_Flag_When_Milk_Present()
    {
        var preference = CreatePreference(dietFlags: DietFlagsDto.Empty with { LactoseFree = true });

        var result = PersonalWarningCalculator.Calculate(preference, Array.Empty<string>(), new[] { "milk" }, null);

        var warning = Assert.Single(result);
        Assert.Equal("lactoseFree", warning.Code);
    }

    [Fact]
    public void Calculate_Warns_On_LowSugar_Flag_When_Sugar_Exceeds_Threshold()
    {
        var preference = CreatePreference(dietFlags: DietFlagsDto.Empty with { LowSugar = true });
        var nutriments = new NutrimentsDto(null, null, null, null, Sugars100g: 30m, null, null, null);

        var result = PersonalWarningCalculator.Calculate(preference, Array.Empty<string>(), Array.Empty<string>(), nutriments);

        var warning = Assert.Single(result);
        Assert.Equal("lowSugar", warning.Code);
    }

    [Fact]
    public void Calculate_Does_Not_Warn_On_LowSugar_Flag_When_Sugar_Under_Threshold()
    {
        var preference = CreatePreference(dietFlags: DietFlagsDto.Empty with { LowSugar = true });
        var nutriments = new NutrimentsDto(null, null, null, null, Sugars100g: 5m, null, null, null);

        var result = PersonalWarningCalculator.Calculate(preference, Array.Empty<string>(), Array.Empty<string>(), nutriments);

        Assert.Empty(result);
    }

    [Fact]
    public void Calculate_Warns_On_LowSalt_Flag_When_Salt_Exceeds_Threshold()
    {
        var preference = CreatePreference(dietFlags: DietFlagsDto.Empty with { LowSalt = true });
        var nutriments = new NutrimentsDto(null, null, null, null, null, null, null, Salt100g: 2m);

        var result = PersonalWarningCalculator.Calculate(preference, Array.Empty<string>(), Array.Empty<string>(), nutriments);

        var warning = Assert.Single(result);
        Assert.Equal("lowSalt", warning.Code);
    }

    [Fact]
    public void Calculate_Combines_Multiple_Warning_Types()
    {
        var preference = CreatePreference(
            avoidedAdditiveCodes: new[] { "E211" },
            allergenCodes: new[] { "milk" },
            dietFlags: DietFlagsDto.Empty with { LowSugar = true });
        var nutriments = new NutrimentsDto(null, null, null, null, Sugars100g: 40m, null, null, null);

        var result = PersonalWarningCalculator.Calculate(preference, new[] { "E211" }, new[] { "milk" }, nutriments);

        Assert.Equal(3, result.Count);
        Assert.Contains(result, w => w.Type == "allergen" && w.Code == "milk");
        Assert.Contains(result, w => w.Type == "additive" && w.Code == "E211");
        Assert.Contains(result, w => w.Type == "diet" && w.Code == "lowSugar");
    }
}
