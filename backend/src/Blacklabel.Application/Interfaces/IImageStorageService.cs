using Blacklabel.Application.ExternalModels;

namespace Blacklabel.Application.Interfaces;

public interface IImageStorageService
{
    Task<string> SaveAsync(Guid contributionId, ContributionImage image, CancellationToken ct);
}
