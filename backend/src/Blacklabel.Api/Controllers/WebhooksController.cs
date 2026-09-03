using System.Text.Json;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Blacklabel.Api.Controllers;

[ApiController]
[Route("api/v1/webhooks")]
[AllowAnonymous]
public class WebhooksController : ControllerBase
{
    private readonly ISubscriptionWebhookService _subscriptionWebhookService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WebhooksController> _logger;

    public WebhooksController(
        ISubscriptionWebhookService subscriptionWebhookService,
        IConfiguration configuration,
        ILogger<WebhooksController> logger)
    {
        _subscriptionWebhookService = subscriptionWebhookService;
        _configuration = configuration;
        _logger = logger;
    }

    // Reads and parses the body manually rather than binding [FromBody] RevenueCatWebhookPayload
    // directly: ASP.NET's automatic model validation treats non-nullable reference-typed
    // properties as implicitly required (this project builds with <Nullable>enable</Nullable>),
    // so any RevenueCat payload shape we didn't anticipate returned a bare 400 before our own
    // code -- and therefore our own logging -- ever ran, leaving no record of what was actually
    // sent. Malformed/unexpected payloads are now logged with their raw body and acknowledged
    // with 200 instead, since RevenueCat retries on non-2xx and a bad payload will never parse
    // differently on retry.
    [HttpPost("revenuecat")]
    public async Task<IActionResult> RevenueCat(CancellationToken ct)
    {
        var configuredSecret = _configuration["RevenueCat:WebhookSecret"];
        var providedSecret = Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(configuredSecret) || providedSecret != configuredSecret)
        {
            return Unauthorized();
        }

        using var reader = new StreamReader(Request.Body);
        var rawBody = await reader.ReadToEndAsync(ct);

        RevenueCatWebhookPayload? payload;
        try
        {
            payload = JsonSerializer.Deserialize<RevenueCatWebhookPayload>(rawBody);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Failed to parse RevenueCat webhook payload: {RawBody}", rawBody);
            return Ok();
        }

        if (payload?.Event is null || string.IsNullOrWhiteSpace(payload.Event.Type) || string.IsNullOrWhiteSpace(payload.Event.AppUserId))
        {
            _logger.LogWarning("RevenueCat webhook payload missing type/app_user_id: {RawBody}", rawBody);
            return Ok();
        }

        await _subscriptionWebhookService.ProcessAsync(payload.Event, ct);

        return Ok();
    }
}
