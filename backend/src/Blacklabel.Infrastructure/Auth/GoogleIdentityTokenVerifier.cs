using Microsoft.Extensions.Logging;

namespace Blacklabel.Infrastructure.Auth;

public sealed class GoogleIdentityTokenVerifier : OidcIdentityTokenVerifier
{
    public GoogleIdentityTokenVerifier(string? clientId, ILogger<GoogleIdentityTokenVerifier> logger)
        : base(
            "https://accounts.google.com/.well-known/openid-configuration",
            new[] { "https://accounts.google.com", "accounts.google.com" },
            clientId,
            logger)
    {
    }

    public override string Provider => "google";
}
