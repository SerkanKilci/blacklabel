using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Interfaces;

public interface IContributionRepository
{
    Task<int> CountSinceAsync(Guid userId, DateTime sinceUtc, CancellationToken ct);
    Task AddAsync(Contribution contribution, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
