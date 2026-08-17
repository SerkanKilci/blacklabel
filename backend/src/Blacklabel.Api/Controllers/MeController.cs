using System.Security.Claims;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.Interfaces;
using Blacklabel.Application.Mapping;
using Blacklabel.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Blacklabel.Api.Controllers;

[ApiController]
[Route("api/v1/me")]
[Authorize]
public class MeController : ControllerBase
{
    private readonly IUserPreferenceRepository _preferenceRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IWebHostEnvironment _environment;

    public MeController(
        IUserPreferenceRepository preferenceRepository,
        IAppUserRepository appUserRepository,
        IWebHostEnvironment environment)
    {
        _preferenceRepository = preferenceRepository;
        _appUserRepository = appUserRepository;
        _environment = environment;
    }

    [HttpGet("preferences")]
    public async Task<IActionResult> GetPreferences(CancellationToken ct)
    {
        var userId = GetUserId();
        var preference = await _preferenceRepository.GetByUserIdAsync(userId, ct);

        return Ok(preference is null
            ? new UserPreferenceResponse(Array.Empty<string>(), Array.Empty<string>(), DietFlagsDto.Empty)
            : UserPreferenceMapper.ToResponse(preference));
    }

    [HttpPut("preferences")]
    public async Task<IActionResult> UpdatePreferences([FromBody] UpdateUserPreferenceRequest request, CancellationToken ct)
    {
        var userId = GetUserId();
        var preference = await _preferenceRepository.GetByUserIdAsync(userId, ct);

        if (preference is null)
        {
            preference = new UserPreference { UserId = userId };
            UserPreferenceMapper.ApplyToEntity(preference, request);
            await _preferenceRepository.AddAsync(preference, ct);
        }
        else
        {
            UserPreferenceMapper.ApplyToEntity(preference, request);
        }

        await _preferenceRepository.SaveChangesAsync(ct);

        return Ok(UserPreferenceMapper.ToResponse(preference));
    }

    [HttpGet("subscription")]
    public async Task<IActionResult> GetSubscription(CancellationToken ct)
    {
        var userId = GetUserId();
        var user = await _appUserRepository.GetByIdAsync(userId, ct);

        return Ok(new SubscriptionResponse(user?.IsPremium ?? false, user?.PremiumUntil));
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(CancellationToken ct)
    {
        var userId = GetUserId();
        var user = await _appUserRepository.GetByIdAsync(userId, ct);

        return Ok(new ProfileResponse(
            user?.Email,
            HasAppleLink: !string.IsNullOrWhiteSpace(user?.AppleUserId),
            HasGoogleLink: !string.IsNullOrWhiteSpace(user?.GoogleUserId)));
    }

    /// <summary>
    /// Deletes the current account (App Store Guideline 5.1.1(v): apps that support account
    /// creation must also support in-app account deletion). Cascade-deletes UserPreference,
    /// Scans, and Contributions via the FK configuration in BlacklabelDbContext. Uploaded
    /// contribution images on disk are not cleaned up as part of this — a known gap, not scoped
    /// to the minimum "delete my account and data" requirement.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteAccount(CancellationToken ct)
    {
        var userId = GetUserId();
        var user = await _appUserRepository.GetByIdAsync(userId, ct);
        if (user is null)
        {
            return NoContent();
        }

        await _appUserRepository.RemoveAsync(user, ct);
        await _appUserRepository.SaveChangesAsync(ct);

        return NoContent();
    }

    /// <summary>
    /// Dev-only convenience so premium-gated screens (Compare, alternatives, personal warnings)
    /// can be tested manually without a real RevenueCat purchase — grants the current account
    /// premium for a year. Returns 404 outside Development regardless of any other config, so
    /// this can never be reachable from a real build.
    /// </summary>
    [HttpPost("debug-grant-premium")]
    public async Task<IActionResult> DebugGrantPremium(CancellationToken ct)
    {
        if (!_environment.IsDevelopment())
        {
            return NotFound();
        }

        var userId = GetUserId();
        var user = await _appUserRepository.GetByIdAsync(userId, ct);
        if (user is null)
        {
            return NotFound();
        }

        user.IsPremium = true;
        user.PremiumUntil = DateTime.UtcNow.AddYears(1);
        await _appUserRepository.SaveChangesAsync(ct);

        return Ok(new SubscriptionResponse(user.IsPremium, user.PremiumUntil));
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
