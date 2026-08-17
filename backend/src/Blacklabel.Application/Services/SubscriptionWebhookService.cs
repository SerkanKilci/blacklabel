using Blacklabel.Application.Dtos;
using Blacklabel.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Blacklabel.Application.Services;

public class SubscriptionWebhookService : ISubscriptionWebhookService
{
    private static readonly HashSet<string> GrantingEventTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "INITIAL_PURCHASE", "RENEWAL", "UNCANCELLATION", "PRODUCT_CHANGE", "TRANSFER"
    };

    private static readonly HashSet<string> RevokingEventTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "EXPIRATION"
    };

    private readonly IAppUserRepository _appUserRepository;
    private readonly ILogger<SubscriptionWebhookService> _logger;

    public SubscriptionWebhookService(IAppUserRepository appUserRepository, ILogger<SubscriptionWebhookService> logger)
    {
        _appUserRepository = appUserRepository;
        _logger = logger;
    }

    public async Task ProcessAsync(RevenueCatEvent webhookEvent, CancellationToken ct)
    {
        if (!Guid.TryParse(webhookEvent.AppUserId, out var userId))
        {
            _logger.LogWarning("RevenueCat webhook app_user_id {AppUserId} is not a recognizable user id", webhookEvent.AppUserId);
            return;
        }

        var user = await _appUserRepository.GetByIdAsync(userId, ct);
        if (user is null)
        {
            _logger.LogWarning("RevenueCat webhook referenced unknown user {UserId}", userId);
            return;
        }

        var expiresAt = webhookEvent.ExpirationAtMs is { } ms
            ? DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime
            : (DateTime?)null;

        if (GrantingEventTypes.Contains(webhookEvent.Type))
        {
            user.IsPremium = true;
            user.PremiumUntil = expiresAt;
            await _appUserRepository.SaveChangesAsync(ct);
        }
        else if (RevokingEventTypes.Contains(webhookEvent.Type))
        {
            user.IsPremium = false;
            user.PremiumUntil = expiresAt;
            await _appUserRepository.SaveChangesAsync(ct);
        }
        else
        {
            _logger.LogInformation("Ignoring RevenueCat event type {EventType} for user {UserId}", webhookEvent.Type, userId);
        }
    }
}
