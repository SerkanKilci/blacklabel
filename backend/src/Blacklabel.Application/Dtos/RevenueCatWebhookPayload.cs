using System.Text.Json.Serialization;

namespace Blacklabel.Application.Dtos;

public sealed record RevenueCatWebhookPayload(
    [property: JsonPropertyName("event")] RevenueCatEvent Event
);

public sealed record RevenueCatEvent(
    [property: JsonPropertyName("type")] string? Type,
    [property: JsonPropertyName("app_user_id")] string? AppUserId,
    [property: JsonPropertyName("expiration_at_ms")] long? ExpirationAtMs
);
