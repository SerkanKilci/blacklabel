using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;

namespace Blacklabel.Infrastructure.Storage;

public class LocalImageStorageService : IImageStorageService
{
    private readonly ImageStorageOptions _options;

    public LocalImageStorageService(ImageStorageOptions options)
    {
        _options = options;
    }

    public async Task<string> SaveAsync(Guid contributionId, ContributionImage image, CancellationToken ct)
    {
        var contributionFolder = Path.Combine(_options.RootPath, contributionId.ToString());
        Directory.CreateDirectory(contributionFolder);

        var fileName = $"{image.Slot}{GetExtension(image.ContentType)}";
        var filePath = Path.Combine(contributionFolder, fileName);

        await File.WriteAllBytesAsync(filePath, image.Content, ct);

        return $"{_options.PublicPathPrefix}/{contributionId}/{fileName}";
    }

    private static string GetExtension(string contentType) => contentType switch
    {
        "image/png" => ".png",
        "image/webp" => ".webp",
        _ => ".jpg"
    };
}
