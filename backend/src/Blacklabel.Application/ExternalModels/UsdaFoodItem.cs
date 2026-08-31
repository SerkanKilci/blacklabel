using Blacklabel.Application.Dtos;

namespace Blacklabel.Application.ExternalModels;

public sealed record UsdaFoodItem(
    string GtinUpc,
    string? IngredientsText,
    NutrimentsDto Nutriments
);
