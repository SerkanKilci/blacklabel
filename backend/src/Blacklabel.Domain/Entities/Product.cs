using Blacklabel.Domain.Enums;

namespace Blacklabel.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Quantity { get; set; }
    public string? IngredientsText { get; set; }
    public string? IngredientsTextTr { get; set; }
    public int? NovaGroup { get; set; }
    public string? NutriScore { get; set; }
    public string? Nutriments { get; set; }
    public string? ImageUrl { get; set; }
    public string Categories { get; set; } = "[]";
    public ProductSource Source { get; set; }
    public DataQuality DataQuality { get; set; }

    /// <summary>
    /// When a USDA FoodData Central lookup was last attempted to fill gaps left by Open Food
    /// Facts. Null means never attempted. Set on every attempt (match found or not) so a product
    /// USDA genuinely has no data for isn't re-queried on every subsequent scan/refresh.
    /// </summary>
    public DateTime? UsdaEnrichedAt { get; set; }
    public int? Score { get; set; }
    public DateTime? ScoreCalculatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<ProductAdditive> ProductAdditives { get; set; } = new List<ProductAdditive>();
    public ICollection<ProductAllergen> ProductAllergens { get; set; } = new List<ProductAllergen>();
}
