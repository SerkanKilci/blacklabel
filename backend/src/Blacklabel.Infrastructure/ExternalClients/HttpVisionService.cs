using System.Net.Http.Json;
using System.Text.Json;
using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Blacklabel.Infrastructure.ExternalClients;

public class HttpVisionService : IVisionService
{
    private const string SystemPrompt =
        """
        You are a food label parser. Read the ingredient list and nutrition table
        from the provided images. Return ONLY valid JSON, no markdown, no prose.
        If a field is not visible in the images, use null. Never guess or infer
        values that are not printed on the package.

        {
          "productName": string|null,
          "brand": string|null,
          "quantity": string|null,
          "ingredientsText": string|null,
          "additiveCodes": string[],          // "E330" format, only if printed
          "allergens": string[],              // lowercase english keys
          "nutriments": {
            "energyKcal100g": number|null, "fat100g": number|null,
            "saturatedFat100g": number|null, "carbohydrates100g": number|null,
            "sugars100g": number|null, "fiber100g": number|null,
            "proteins100g": number|null, "salt100g": number|null
          },
          "confidence": number                 // 0..1
        }
        """;

    private const int MaxAttempts = 2;

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpVisionService> _logger;

    public HttpVisionService(HttpClient httpClient, ILogger<HttpVisionService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<LabelExtractionResult?> ExtractLabelAsync(IReadOnlyList<ContributionImage> images, CancellationToken ct)
    {
        if (_httpClient.BaseAddress is null)
        {
            _logger.LogWarning("Vision service endpoint is not configured; skipping extraction.");
            return null;
        }

        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                var result = await CallEndpointAsync(images, ct);
                if (IsValid(result))
                {
                    return result;
                }

                _logger.LogWarning("Vision service returned an invalid schema on attempt {Attempt}", attempt);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
            {
                _logger.LogWarning(ex, "Vision service call failed on attempt {Attempt}", attempt);
            }
        }

        return null;
    }

    private async Task<LabelExtractionResult?> CallEndpointAsync(IReadOnlyList<ContributionImage> images, CancellationToken ct)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(SystemPrompt), "systemPrompt" },
            { new StringContent("0"), "temperature" }
        };

        foreach (var image in images)
        {
            var imageContent = new ByteArrayContent(image.Content);
            imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
            content.Add(imageContent, image.Slot, image.FileName);
        }

        using var response = await _httpClient.PostAsync(string.Empty, content, ct);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Vision service responded with status {StatusCode}", response.StatusCode);
            return null;
        }

        return await response.Content.ReadFromJsonAsync<LabelExtractionResult>(JsonOptions, ct);
    }

    private static bool IsValid(LabelExtractionResult? result)
    {
        if (result is null)
        {
            return false;
        }

        if (result.Confidence is < 0 or > 1)
        {
            return false;
        }

        return result.AdditiveCodes is not null && result.Allergens is not null && result.Nutriments is not null;
    }
}
