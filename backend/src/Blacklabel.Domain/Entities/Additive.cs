using Blacklabel.Domain.Enums;

namespace Blacklabel.Domain.Entities;

public class Additive
{
    public string Code { get; set; } = string.Empty;
    public string NameTr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameDe { get; set; } = string.Empty;
    public string NameFr { get; set; } = string.Empty;
    public string NameEs { get; set; } = string.Empty;
    public AdditiveCategory Category { get; set; }
    public int RiskLevel { get; set; }
    public string DescriptionTr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionDe { get; set; } = string.Empty;
    public string DescriptionFr { get; set; } = string.Empty;
    public string DescriptionEs { get; set; } = string.Empty;
    public string? SourceNote { get; set; }
    public string Synonyms { get; set; } = "[]";

    public ICollection<ProductAdditive> ProductAdditives { get; set; } = new List<ProductAdditive>();
}
