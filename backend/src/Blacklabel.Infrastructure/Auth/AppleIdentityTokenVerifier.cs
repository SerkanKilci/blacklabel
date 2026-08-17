using Microsoft.Extensions.Logging;

namespace Blacklabel.Infrastructure.Auth;

public sealed class AppleIdentityTokenVerifier : OidcIdentityTokenVerifier
{
    public AppleIdentityTokenVerifier(string? bundleId, ILogger<AppleIdentityTokenVerifier> logger)
        : base(
            "https://appleid.apple.com/.well-known/openid-configuration",
            new[] { "https://appleid.apple.com" },
            bundleId,
            logger)
    {
    }

    public override string Provider => "apple";
}
