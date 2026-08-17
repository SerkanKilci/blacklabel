namespace Blacklabel.Application.Dtos;

public sealed record DietFlagsDto(
    bool Vegan,
    bool Vegetarian,
    bool GlutenFree,
    bool LactoseFree,
    bool NoPalmOil,
    bool LowSugar,
    bool LowSalt
)
{
    public static DietFlagsDto Empty { get; } = new(false, false, false, false, false, false, false);
}
