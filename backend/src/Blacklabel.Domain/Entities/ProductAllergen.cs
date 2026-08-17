namespace Blacklabel.Domain.Entities;

public class ProductAllergen
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string AllergenCode { get; set; } = string.Empty;
    public Allergen Allergen { get; set; } = null!;
}
