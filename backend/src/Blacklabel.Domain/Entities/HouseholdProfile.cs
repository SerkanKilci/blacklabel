namespace Blacklabel.Domain.Entities;

public class HouseholdProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string Name { get; set; } = string.Empty;

    public string AvoidedAdditiveCodes { get; set; } = "[]";
    public string AllergenCodes { get; set; } = "[]";
    public string DietFlags { get; set; } = "{}";
    public DateTime CreatedAt { get; set; }
}
