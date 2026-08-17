namespace Blacklabel.Application.Dtos;

public sealed record UserPreferenceResponse(
    IReadOnlyList<string> AvoidedAdditiveCodes,
    IReadOnlyList<string> AllergenCodes,
    DietFlagsDto DietFlags
);

public sealed record UpdateUserPreferenceRequest(
    IReadOnlyList<string> AvoidedAdditiveCodes,
    IReadOnlyList<string> AllergenCodes,
    DietFlagsDto DietFlags
);
