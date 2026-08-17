namespace Blacklabel.Domain.Entities;

public class UserPreference
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public string AvoidedAdditiveCodes { get; set; } = "[]";
    public string AllergenCodes { get; set; } = "[]";
    public string DietFlags { get; set; } = "{}";
}
