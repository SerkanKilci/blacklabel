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
    IReadOnlyList<PersonalWarningDto> PersonalWarnings,
    string DataQuality,
    string Source,
    ComparisonBands ComparisonBands
);

public sealed record ProductNotFoundResponse(bool Found, bool CanContribute);
