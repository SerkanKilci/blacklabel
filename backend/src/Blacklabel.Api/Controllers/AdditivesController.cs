using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Blacklabel.Application.Interfaces;
using Blacklabel.Application.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blacklabel.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class AdditivesController : ControllerBase
{
    private readonly IAdditiveRepository _repository;

    public AdditivesController(IAdditiveRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var additives = await _repository.GetAllAsync(ct);
        var response = additives.Select(AdditiveMapper.ToResponse).ToList();

        var etag = ComputeETag(response);
        if (Request.Headers.IfNoneMatch.Any(value => value == etag))
        {
            return StatusCode(StatusCodes.Status304NotModified);
        }

        Response.Headers.ETag = etag;
        return Ok(response);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code, CancellationToken ct)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();
        var additive = await _repository.GetByCodeAsync(normalizedCode, ct);
        if (additive is null)
        {
            return NotFound();
        }

        return Ok(AdditiveMapper.ToResponse(additive));
    }

    private static string ComputeETag(object value)
    {
        var json = JsonSerializer.Serialize(value);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(json));
        return $"\"{Convert.ToHexString(hash)}\"";
    }
}
