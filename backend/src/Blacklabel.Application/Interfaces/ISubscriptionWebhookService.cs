using Blacklabel.Application.Dtos;

namespace Blacklabel.Application.Interfaces;

public interface ISubscriptionWebhookService
{
    Task ProcessAsync(RevenueCatEvent webhookEvent, CancellationToken ct);
}
