using Blacklabel.Application.Dtos;

namespace Blacklabel.Application.ExternalModels;

public sealed record LabelExtractionResult(
    string? ProductName,
    string? Brand,
    string? Quantity,
    string? IngredientsText,
    IReadOnlyList<string> AdditiveCodes,
    IReadOnlyList<string> Allergens,
    NutrimentsDto Nutriments,
    double Confidence
);
