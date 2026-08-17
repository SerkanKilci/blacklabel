using Blacklabel.Application.Dtos;
using Blacklabel.Application.Interfaces;
using Blacklabel.Application.Services;
using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blacklabel.Tests.Services;

public class AccountLinkServiceTests
{
    private class FakeTokenService : ITokenService
    {
        public string GenerateToken(AppUser user) => $"token-for-{user.Id}";
    }

    // Mimics a verifier that only accepts a fixed, "correctly signed" token for its provider —
    // real JWKS signature/issuer/audience verification is Infrastructure's job and isn't
    // exercised here; this covers AccountLinkService's own branching (unknown user, unsupported
    // provider, rejected token, claims applied on success).
    private class FakeIdentityTokenVerifier : IIdentityTokenVerifier
    {
        private readonly string _acceptedToken;
        private readonly VerifiedIdentity _identity;

        public FakeIdentityTokenVerifier(string provider, string acceptedToken, VerifiedIdentity identity)
        {
            Provider = provider;
            _acceptedToken = acceptedToken;
            _identity = identity;
        }

        public string Provider { get; }

        public Task<VerifiedIdentity?> VerifyAsync(string identityToken, CancellationToken ct)
            => Task.FromResult(identityToken == _acceptedToken ? _identity : null);
    }

    private static (BlacklabelDbContext Context, AccountLinkService Service, Guid UserId) CreateService()
    {
        var options = new DbContextOptionsBuilder<BlacklabelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BlacklabelDbContext(options);
        context.Database.EnsureCreated();

        var userId = Guid.NewGuid();
        context.AppUsers.Add(new AppUser { Id = userId, DeviceId = "device-1", CreatedAt = DateTime.UtcNow });
        context.SaveChanges();

        var verifiers = new IIdentityTokenVerifier[]
        {
            new FakeIdentityTokenVerifier("apple", "valid-apple-token", new VerifiedIdentity("apple-sub-1", "user@example.com")),
            new FakeIdentityTokenVerifier("google", "valid-google-token", new VerifiedIdentity("google-sub-1", null))
        };

        var service = new AccountLinkService(new AppUserRepository(context), new FakeTokenService(), verifiers);
        return (context, service, userId);
    }

    [Fact]
    public async Task LinkAsync_Returns_Null_For_Unknown_User()
    {
        var (_, service, _) = CreateService();

        var result = await service.LinkAsync(Guid.NewGuid(), new LinkAccountRequest("apple", "valid-apple-token"), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task LinkAsync_Returns_Null_For_Unsupported_Provider()
    {
        var (_, service, userId) = CreateService();

        var result = await service.LinkAsync(userId, new LinkAccountRequest("facebook", "anything"), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task LinkAsync_Returns_Null_When_Token_Fails_Verification()
    {
        var (_, service, userId) = CreateService();

        var result = await service.LinkAsync(userId, new LinkAccountRequest("apple", "forged-token"), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task LinkAsync_Sets_AppleUserId_And_Email_From_Verified_Claims()
    {
        var (context, service, userId) = CreateService();

        var result = await service.LinkAsync(userId, new LinkAccountRequest("apple", "valid-apple-token"), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal($"token-for-{userId}", result!.Token);

        var user = await context.AppUsers.SingleAsync(u => u.Id == userId);
        Assert.Equal("apple-sub-1", user.AppleUserId);
        Assert.Equal("user@example.com", user.Email);
        Assert.Null(user.GoogleUserId);
    }

    [Fact]
    public async Task LinkAsync_Sets_GoogleUserId_And_Preserves_Existing_Email_When_Token_Has_None()
    {
        var (context, service, userId) = CreateService();
        var user = await context.AppUsers.SingleAsync(u => u.Id == userId);
        user.Email = "already-set@example.com";
        await context.SaveChangesAsync();

        var result = await service.LinkAsync(userId, new LinkAccountRequest("google", "valid-google-token"), CancellationToken.None);

        Assert.NotNull(result);
        var updated = await context.AppUsers.SingleAsync(u => u.Id == userId);
        Assert.Equal("google-sub-1", updated.GoogleUserId);
        Assert.Equal("already-set@example.com", updated.Email);
    }
}
