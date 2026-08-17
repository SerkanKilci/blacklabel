using Blacklabel.Application.Dtos;
using Blacklabel.Application.Interfaces;

namespace Blacklabel.Application.Services;

public class AccountLinkService : IAccountLinkService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly ITokenService _tokenService;
    private readonly IReadOnlyDictionary<string, IIdentityTokenVerifier> _verifiersByProvider;

    public AccountLinkService(
        IAppUserRepository appUserRepository,
        ITokenService tokenService,
        IEnumerable<IIdentityTokenVerifier> verifiers)
    {
        _appUserRepository = appUserRepository;
        _tokenService = tokenService;
        _verifiersByProvider = verifiers.ToDictionary(v => v.Provider, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<AuthResponse?> LinkAsync(Guid userId, LinkAccountRequest request, CancellationToken ct)
    {
        var user = await _appUserRepository.GetByIdAsync(userId, ct);
        if (user is null)
        {
            return null;
        }

        var provider = request.Provider.ToLowerInvariant();
        if (!_verifiersByProvider.TryGetValue(provider, out var verifier))
        {
            return null;
        }

        // The identity token is verified against the provider's live JWKS (signature, issuer,
        // audience, expiry) — only claims from a token that passes this are ever trusted.
        var identity = await verifier.VerifyAsync(request.IdentityToken, ct);
        if (identity is null)
        {
            return null;
        }

        switch (provider)
        {
            case "apple":
                user.AppleUserId = identity.ProviderUserId;
                break;
            case "google":
                user.GoogleUserId = identity.ProviderUserId;
                break;
            default:
                return null;
        }

        if (!string.IsNullOrWhiteSpace(identity.Email))
        {
            user.Email = identity.Email;
        }

        await _appUserRepository.SaveChangesAsync(ct);

        var token = _tokenService.GenerateToken(user);
        return new AuthResponse(token, user.Id, user.IsPremium);
    }
}
