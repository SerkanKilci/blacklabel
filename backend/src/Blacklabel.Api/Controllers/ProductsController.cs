using System.Security.Claims;
using Blacklabel.Application.Dtos;
using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blacklabel.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductLookupService _lookupService;
    private readonly IContributionService _contributionService;

    public ProductsController(IProductLookupService lookupService, IContributionService contributionService)
    {
        _lookupService = lookupService;
        _contributionService = contributionService;
    }

    [HttpGet("{barcode}")]
    public async Task<IActionResult> GetByBarcode(string barcode, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await _lookupService.GetByBarcodeAsync(userId, barcode, ct);

        return result.Outcome switch
        {
            ProductLookupOutcome.Found => Ok(result.Product),
            ProductLookupOutcome.DailyLimitExceeded => StatusCode(
                StatusCodes.Status429TooManyRequests,
                new { message = "Daily scan limit reached. Upgrade to premium for unlimited scans." }),
            _ => NotFound(new ProductNotFoundResponse(false, result.CanContribute))
        };
    }

    [HttpGet("{barcode}/alternatives")]
    public async Task<IActionResult> GetAlternatives(string barcode, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await _lookupService.GetAlternativesAsync(userId, barcode, ct);

        return result.Outcome switch
        {
            AlternativesOutcome.Found => Ok(result.Alternatives),
            AlternativesOutcome.PremiumRequired => StatusCode(
                StatusCodes.Status403Forbidden,
                new { message = "Alternative product suggestions are a premium feature." }),
            _ => NotFound()
        };
    }

    [HttpPost("{barcode}/contribute")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(30_000_000)]
    public async Task<IActionResult> Contribute(
        string barcode,
        IFormFile? front,
        IFormFile? ingredients,
        IFormFile? nutrition,
        CancellationToken ct)
    {
        if (front is null || ingredients is null || nutrition is null)
        {
            return BadRequest(new { message = "front, ingredients and nutrition images are all required." });
        }

        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var images = new List<ContributionImage>();
        foreach (var (slot, file) in new[] { ("front", front), ("ingredients", ingredients), ("nutrition", nutrition) })
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, ct);
            images.Add(new ContributionImage(slot, file.FileName, file.ContentType ?? "image/jpeg", stream.ToArray()));
        }

        var result = await _contributionService.SubmitAsync(userId, barcode, images, ct);

        return result.Outcome switch
        {
            ContributionOutcome.InvalidBarcode => BadRequest(new { message = "Invalid barcode." }),
            ContributionOutcome.DailyLimitExceeded => StatusCode(
                StatusCodes.Status429TooManyRequests,
                new { message = "Daily label-analysis limit reached. Upgrade to premium for unlimited analysis." }),
            ContributionOutcome.VisionFailed => StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = "Failed" }),
            _ => Ok(result.Product)
        };
    }

    private bool TryGetUserId(out Guid userId)
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
}
