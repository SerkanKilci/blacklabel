using Blacklabel.Application.ExternalModels;
using Blacklabel.Application.Interfaces;

namespace Blacklabel.Tests.ExternalClients;

public class FakeImageStorageService : IImageStorageService
{
    public Task<string> SaveAsync(Guid contributionId, ContributionImage image, CancellationToken ct)
        => Task.FromResult($"/uploads/contributions/{contributionId}/{image.Slot}.jpg");
}
