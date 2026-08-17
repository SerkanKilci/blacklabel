using Blacklabel.Application.Dtos;
using Blacklabel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Blacklabel.Api.Controllers;

[ApiController]
[Route("api/v1/webhooks")]
[AllowAnonymous]
public class WebhooksController : ControllerBase
{
    private readonly ISubscriptionWebhookService _subscriptionWebhookService;
    private readonly IConfiguration _configuration;

    public WebhooksController(ISubscriptionWebhookService subscriptionWebhookService, IConfiguration configuration)
    {
        _subscriptionWebhookService = subscriptionWebhookService;
        _configuration = configuration;
    }

    [HttpPost("revenuecat")]
    public async Task<IActionResult> RevenueCat([FromBody] RevenueCatWebhookPayload payload, CancellationToken ct)
    {
        var configuredSecret = _configuration["RevenueCat:WebhookSecret"];
        var providedSecret = Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(configuredSecret) || providedSecret != configuredSecret)
        {
            return Unauthorized();
        }

        await _subscriptionWebhookService.ProcessAsync(payload.Event, ct);

        return Ok();
    }
}
