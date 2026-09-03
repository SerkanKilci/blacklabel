using Blacklabel.Application.Dtos;
using Blacklabel.Application.Interfaces;
using Blacklabel.Domain.Entities;

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

        // If this identity is already linked to a different account -- the common "signed in
        // before, now on a new device/reinstall" case -- switch to that account instead of
        // stamping the same provider id onto a second row. Without this, a returning user's scan
        // history and household profiles stay stranded on whichever fresh anonymous account they
        // happened to open the app with this time; only their premium entitlement would come back
        // (that part works independently, via RevenueCat's own receipt-based restore).
        AppUser? existingUser = provider switch
        {
            "apple" => await _appUserRepository.GetByAppleUserIdAsync(identity.ProviderUserId, ct),
            "google" => await _appUserRepository.GetByGoogleUserIdAsync(identity.ProviderUserId, ct),
            _ => null
        };

        var targetUser = existingUser is not null && existingUser.Id != user.Id ? existingUser : user;

        switch (provider)
        {
            case "apple":
                targetUser.AppleUserId = identity.ProviderUserId;
                break;
            case "google":
                targetUser.GoogleUserId = identity.ProviderUserId;
                break;
            default:
                return null;
        }

        if (!string.IsNullOrWhiteSpace(identity.Email))
        {
            targetUser.Email = identity.Email;
        }

        await _appUserRepository.SaveChangesAsync(ct);

        var token = _tokenService.GenerateToken(targetUser);
        return new AuthResponse(token, targetUser.Id, targetUser.IsPremium);
    }
}
