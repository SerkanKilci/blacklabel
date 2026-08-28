namespace Blacklabel.Application.Dtos;

public sealed record ProductResponse(
    bool Found,
    string Barcode,
    string Name,
    string? Brand,
    string? ImageUrl,
    int? Score,
    ScoreBreakdownDto? ScoreBreakdown,
    int? NovaGroup,
    string? NutriScore,
    string? IngredientsText,
    IReadOnlyList<AdditiveResponse> Additives,
    IReadOnlyList<string> Allergens,
    NutrimentsDto? Nutriments,
    IReadOnlyList<ProfileWarningDto> ProfileWarnings,
    string DataQuality,
    string Source,
    ComparisonBands ComparisonBands,
    bool HasLockedPersonalWarnings = false
);

public sealed record ProductNotFoundResponse(bool Found, bool CanContribute);
