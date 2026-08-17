namespace Blacklabel.Application.Dtos;

public sealed record SubscriptionResponse(bool IsPremium, DateTime? PremiumUntil);
