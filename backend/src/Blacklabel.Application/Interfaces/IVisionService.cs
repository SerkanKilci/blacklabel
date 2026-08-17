using Blacklabel.Application.ExternalModels;

namespace Blacklabel.Application.Interfaces;

public interface IVisionService
{
    Task<LabelExtractionResult?> ExtractLabelAsync(IReadOnlyList<ContributionImage> images, CancellationToken ct);
}
