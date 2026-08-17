namespace Blacklabel.Domain.Entities;

public class Allergen
{
    public string Code { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;

    public ICollection<ProductAllergen> ProductAllergens { get; set; } = new List<ProductAllergen>();
}
