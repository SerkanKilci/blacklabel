using System.Security.Claims;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blacklabel.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IDeviceAuthService _deviceAuthService;
    private readonly IAccountLinkService _accountLinkService;

    public AuthController(IDeviceAuthService deviceAuthService, IAccountLinkService accountLinkService)
    {
        _deviceAuthService = deviceAuthService;
        _accountLinkService = accountLinkService;
    }

    [HttpPost("device")]
    public async Task<IActionResult> Device([FromBody] DeviceAuthRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.DeviceId))
        {
            return BadRequest();
        }

        var response = await _deviceAuthService.AuthenticateAsync(request.DeviceId, ct);
        return Ok(response);
    }

    [HttpPost("link")]
    [Authorize]
    public async Task<IActionResult> Link([FromBody] LinkAccountRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Provider) || string.IsNullOrWhiteSpace(request.IdentityToken))
        {
            return BadRequest();
        }

        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var response = await _accountLinkService.LinkAsync(userId, request, ct);
        if (response is null)
        {
            return BadRequest(new { message = "Unknown user, unsupported provider, or invalid identity token." });
        }

        return Ok(response);
    }
}
