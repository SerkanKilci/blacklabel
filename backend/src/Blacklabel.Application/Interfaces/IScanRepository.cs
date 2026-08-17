using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Interfaces;

public interface IScanRepository
{
    Task<(IReadOnlyList<Scan> Items, int TotalCount)> GetPagedByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct);
    Task<int> CountSinceAsync(Guid userId, DateTime sinceUtc, CancellationToken ct);
    Task AddAsync(Scan scan, CancellationToken ct);
    Task AddRangeAsync(IEnumerable<Scan> scans, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
