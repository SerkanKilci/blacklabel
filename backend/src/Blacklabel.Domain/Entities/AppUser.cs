namespace Blacklabel.Domain.Entities;

public class AppUser
{
    public Guid Id { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string? AppleUserId { get; set; }
    public string? GoogleUserId { get; set; }
    public string? Email { get; set; }
    public bool IsPremium { get; set; }
    public DateTime? PremiumUntil { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserPreference? UserPreference { get; set; }
    public ICollection<Scan> Scans { get; set; } = new List<Scan>();
    public ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();
}
