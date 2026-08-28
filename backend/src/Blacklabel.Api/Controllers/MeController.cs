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
    private readonly IHouseholdProfileRepository _profileRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IAdditiveRepository _additiveRepository;
    private readonly IAllergenRepository _allergenRepository;
    private readonly IWebHostEnvironment _environment;

    public MeController(
        IHouseholdProfileRepository profileRepository,
        IAppUserRepository appUserRepository,
        IAdditiveRepository additiveRepository,
        IAllergenRepository allergenRepository,
        IWebHostEnvironment environment)
    {
        _profileRepository = profileRepository;
        _appUserRepository = appUserRepository;
        _additiveRepository = additiveRepository;
        _allergenRepository = allergenRepository;
        _environment = environment;
    }

    /// <summary>
    /// Lists the household profiles (§CBS/Yuka critique: one profile per person sharing the
    /// account, e.g. an allergic child alongside a parent on a low-salt diet) so a single scan
    /// can be evaluated once per person instead of one blended result for the whole household.
    /// </summary>
    [HttpGet("household-profiles")]
    public async Task<IActionResult> GetHouseholdProfiles(CancellationToken ct)
    {
        var userId = GetUserId();
        var profiles = await _profileRepository.GetByUserIdAsync(userId, ct);
        return Ok(profiles.Select(HouseholdProfileMapper.ToResponse).ToList());
    }

    [HttpPost("household-profiles")]
    public async Task<IActionResult> CreateHouseholdProfile([FromBody] CreateHouseholdProfileRequest request, CancellationToken ct)
    {
        var userId = GetUserId();
        var user = await _appUserRepository.GetByIdAsync(userId, ct);
        if (user is null || !user.IsPremium)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Household profiles are a premium feature." });
        }

        var profile = new HouseholdProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        await _profileRepository.AddAsync(profile, ct);
        await _profileRepository.SaveChangesAsync(ct);

        return Ok(HouseholdProfileMapper.ToResponse(profile));
    }

    [HttpPut("household-profiles/{profileId:guid}")]
    public async Task<IActionResult> UpdateHouseholdProfile(
        Guid profileId, [FromBody] UpdateHouseholdProfileRequest request, CancellationToken ct)
    {
        var userId = GetUserId();
        var user = await _appUserRepository.GetByIdAsync(userId, ct);
        if (user is null || !user.IsPremium)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Household profiles are a premium feature." });
        }

        var profile = await _profileRepository.GetByIdAsync(profileId, userId, ct);
        if (profile is null)
        {
            return NotFound();
        }

        foreach (var code in request.AvoidedAdditiveCodes)
        {
            if (await _additiveRepository.GetByCodeAsync(code, ct) is null)
            {
                return BadRequest($"'{code}' is not a known additive code.");
            }
        }

        foreach (var code in request.AllergenCodes)
        {
            if (await _allergenRepository.GetByCodeAsync(code, ct) is null)
            {
                return BadRequest($"'{code}' is not a known allergen code.");
            }
        }

        HouseholdProfileMapper.ApplyToEntity(profile, request);
        await _profileRepository.SaveChangesAsync(ct);

        return Ok(HouseholdProfileMapper.ToResponse(profile));
    }

    [HttpDelete("household-profiles/{profileId:guid}")]
    public async Task<IActionResult> DeleteHouseholdProfile(Guid profileId, CancellationToken ct)
    {
        var userId = GetUserId();
        var profile = await _profileRepository.GetByIdAsync(profileId, userId, ct);
        if (profile is null)
        {
            return NotFound();
        }

        _profileRepository.Remove(profile);
        await _profileRepository.SaveChangesAsync(ct);

        return NoContent();
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
    /// creation must also support in-app account deletion). Cascade-deletes HouseholdProfiles,
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
