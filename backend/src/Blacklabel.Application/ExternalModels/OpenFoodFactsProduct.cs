using Blacklabel.Application.Dtos;

namespace Blacklabel.Application.ExternalModels;

public sealed record OpenFoodFactsProduct(
    string? ProductName,
    string? ProductNameTr,
    string? Brands,
    string? Quantity,
    string? IngredientsText,
    string? IngredientsTextTr,
    IReadOnlyList<string> AdditivesTags,
    IReadOnlyList<string> AllergensTags,
    int? NovaGroup,
    string? NutriscoreGrade,
    NutrimentsDto Nutriments,
    string? ImageUrl,
    IReadOnlyList<string> CategoriesTags
);
