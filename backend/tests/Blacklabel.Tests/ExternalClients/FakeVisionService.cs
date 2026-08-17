using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;

namespace Blacklabel.Tests.ExternalClients;

public class FakeVisionService : IVisionService
{
    private readonly LabelExtractionResult? _result;

    public FakeVisionService(LabelExtractionResult? result)
    {
        _result = result;
    }

    public Task<LabelExtractionResult?> ExtractLabelAsync(IReadOnlyList<ContributionImage> images, CancellationToken ct)
        => Task.FromResult(_result);
}
