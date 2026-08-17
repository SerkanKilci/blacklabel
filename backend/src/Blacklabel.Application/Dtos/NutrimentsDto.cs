namespace Blacklabel.Application.Dtos;

public sealed record NutrimentsDto(
    decimal? EnergyKcal100g,
    decimal? Fat100g,
    decimal? SaturatedFat100g,
    decimal? Carbohydrates100g,
    decimal? Sugars100g,
    decimal? Fiber100g,
    decimal? Proteins100g,
    decimal? Salt100g
);
