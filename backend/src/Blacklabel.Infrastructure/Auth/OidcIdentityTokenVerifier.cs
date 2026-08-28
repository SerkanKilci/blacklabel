using System.IdentityModel.Tokens.Jwt;
using Blacklabel.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Blacklabel.Infrastructure.Auth;

/// <summary>
/// Shared verification for OIDC-style native sign-in identity tokens (Sign in with Apple, Google
/// Sign-In): fetches the provider's signing keys from its published discovery document (cached
/// and auto-refreshed by <see cref="ConfigurationManager{T}"/>, the same mechanism ASP.NET Core's
/// own JWT bearer handler uses for OIDC authorities) and validates signature, issuer, audience and
/// expiry before any claim inside the token is trusted.
/// </summary>
public abstract class OidcIdentityTokenVerifier : IIdentityTokenVerifier
{
    private readonly ConfigurationManager<OpenIdConnectConfiguration> _configManager;
    private readonly string[] _validIssuers;
    private readonly string? _audience;
    private readonly ILogger _logger;

    protected OidcIdentityTokenVerifier(
        string metadataAddress, string[] validIssuers, string? audience, ILogger logger)
    {
        _configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            metadataAddress, new OpenIdConnectConfigurationRetriever());
        _validIssuers = validIssuers;
        _audience = audience;
        _logger = logger;
    }

    public abstract string Provider { get; }

    public async Task<VerifiedIdentity?> VerifyAsync(string identityToken, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(_audience))
        {
            // No bundle id / client id configured for this provider yet — fail closed rather
            // than skip audience validation, matching the RevenueCat webhook's "unset secret
            // means reject everything" posture.
            _logger.LogWarning("{Provider} sign-in is not configured (missing audience) — rejecting link request.", Provider);
            return null;
        }

        try
        {
            var config = await _configManager.GetConfigurationAsync(ct);
            var handler = new JwtSecurityTokenHandler
            {
                // Without this, the handler silently rewrites the "sub" claim to the long
                // ClaimTypes.NameIdentifier URI (a legacy WS-Federation compat behavior), so the
                // FindFirst(JwtRegisteredClaimNames.Sub) lookup below always came back null and
                // every otherwise-valid Apple/Google identity token got rejected.
                MapInboundClaims = false
            };
            var parameters = new TokenValidationParameters
            {
                ValidIssuers = _validIssuers,
                ValidateIssuer = true,
                ValidAudience = _audience,
                ValidateAudience = true,
                IssuerSigningKeys = config.SigningKeys,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireSignedTokens = true,
                RequireExpirationTime = true
            };

            var principal = handler.ValidateToken(identityToken, parameters, out _);
            var subject = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrWhiteSpace(subject))
            {
                return null;
            }

            var email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            return new VerifiedIdentity(subject, email);
        }
        catch (Exception ex) when (ex is SecurityTokenException or ArgumentException or InvalidOperationException)
        {
            _logger.LogWarning(ex, "{Provider} identity token verification failed.", Provider);
            return null;
        }
    }
}
