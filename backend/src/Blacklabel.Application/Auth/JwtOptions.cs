namespace Blacklabel.Application.Auth;

public sealed class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "Blacklabel";
    public string Audience { get; set; } = "Blacklabel.Mobile";
    public int ExpiryMinutes { get; set; } = 43200;
}
