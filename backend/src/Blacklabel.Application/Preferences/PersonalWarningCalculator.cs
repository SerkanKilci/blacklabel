using Blacklabel.Application.Dtos;

namespace Blacklabel.Application.Preferences;

public static class PersonalWarningCalculator
{
    // UK FSA "high" thresholds per 100g, reused here since they're already the reference point
    // for the nutrition scoring bands (see ScoreThresholds defaults).
    private const decimal HighSugarThreshold = 22.5m;
    private const decimal HighSaltThreshold = 1.5m;

    public static IReadOnlyList<PersonalWarningDto> Calculate(
        UserPreferenceResponse? preference,
        IReadOnlyList<string> productAdditiveCodes,
        IReadOnlyList<string> productAllergenCodes,
        NutrimentsDto? nutriments)
    {
        if (preference is null)
        {
            return Array.Empty<PersonalWarningDto>();
        }

        var warnings = new List<PersonalWarningDto>();

        foreach (var allergenCode in productAllergenCodes)
        {
            if (preference.AllergenCodes.Contains(allergenCode))
            {
                warnings.Add(new PersonalWarningDto("allergen", allergenCode, $"warning.allergen.{allergenCode}"));
            }
        }

        foreach (var additiveCode in productAdditiveCodes)
        {
            if (preference.AvoidedAdditiveCodes.Contains(additiveCode))
            {
                warnings.Add(new PersonalWarningDto("additive", additiveCode, $"warning.additive.{additiveCode}"));
            }
        }

        // Only diet flags with a reliable signal in our data model are checked here. Vegan,
        // vegetarian and noPalmOil have no backing field (Open Food Facts ingredient-analysis
        // tags aren't fetched — see §5's fixed `fields` list) so they are stored but never warned on.
        if (preference.DietFlags.GlutenFree && productAllergenCodes.Contains("gluten"))
        {
            warnings.Add(new PersonalWarningDto("diet", "glutenFree", "warning.diet.glutenFree"));
        }

        if (preference.DietFlags.LactoseFree && productAllergenCodes.Contains("milk"))
        {
            warnings.Add(new PersonalWarningDto("diet", "lactoseFree", "warning.diet.lactoseFree"));
        }

        if (preference.DietFlags.LowSugar && nutriments?.Sugars100g is { } sugars && sugars > HighSugarThreshold)
        {
            warnings.Add(new PersonalWarningDto("diet", "lowSugar", "warning.diet.lowSugar"));
        }

        if (preference.DietFlags.LowSalt && nutriments?.Salt100g is { } salt && salt > HighSaltThreshold)
        {
            warnings.Add(new PersonalWarningDto("diet", "lowSalt", "warning.diet.lowSalt"));
        }

        return warnings;
    }
}
