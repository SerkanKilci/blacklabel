namespace Blacklabel.Application.Interfaces;

public sealed record VerifiedIdentity(string ProviderUserId, string? Email);

/// <summary>
/// Verifies a native Sign-In-with-Apple/Google identity token server-side (signature against the
/// provider's published JWKS, issuer, audience, expiry) before the claims inside it are trusted.
/// One implementation per provider; <see cref="Provider"/> must match the lowercase provider name
/// used in <see cref="Blacklabel.Application.Dtos.LinkAccountRequest"/>.
/// </summary>
public interface IIdentityTokenVerifier
{
    string Provider { get; }

    Task<VerifiedIdentity?> VerifyAsync(string identityToken, CancellationToken ct);
}
