using Blacklabel.Application.Dtos;
using Blacklabel.Application.ExternalModels;

namespace Blacklabel.Application.Interfaces;

public enum ContributionOutcome
{
    InvalidBarcode,
    DailyLimitExceeded,
    VisionFailed,
    Created,
    ExistingProductUnchanged
}

public sealed record ContributionResult(ContributionOutcome Outcome, ProductResponse? Product);

public interface IContributionService
{
    Task<ContributionResult> SubmitAsync(Guid userId, string rawBarcode, IReadOnlyList<ContributionImage> images, CancellationToken ct);
}
