namespace Blacklabel.Domain.Entities;

public class ProductAdditive
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string AdditiveCode { get; set; } = string.Empty;
    public Additive Additive { get; set; } = null!;
}
