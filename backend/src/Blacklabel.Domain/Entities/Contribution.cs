using Blacklabel.Domain.Enums;

namespace Blacklabel.Domain.Entities;

public class Contribution
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public string Barcode { get; set; } = string.Empty;
    public string ImageUrls { get; set; } = "[]";
    public string? RawVisionOutput { get; set; }
    public ContributionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
