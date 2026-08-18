namespace Blacklabel.Application.Dtos;

public sealed record HouseholdProfileResponse(
    Guid Id,
    string Name,
    IReadOnlyList<string> AvoidedAdditiveCodes,
    IReadOnlyList<string> AllergenCodes,
    DietFlagsDto DietFlags
);

public sealed record CreateHouseholdProfileRequest(string Name);

public sealed record UpdateHouseholdProfileRequest(
    string Name,
    IReadOnlyList<string> AvoidedAdditiveCodes,
    IReadOnlyList<string> AllergenCodes,
    DietFlagsDto DietFlags
);

/// <summary>
/// The preference fields alone, used internally to feed <see cref="Preferences.PersonalWarningCalculator"/>
/// once per household profile — not exposed directly by any endpoint.
/// </summary>
public sealed record UserPreferenceResponse(
    IReadOnlyList<string> AvoidedAdditiveCodes,
    IReadOnlyList<string> AllergenCodes,
    DietFlagsDto DietFlags
);
