using Blacklabel.Application.Interfaces;
using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blacklabel.Infrastructure.Repositories;

public class ScanRepository : IScanRepository
{
    private readonly BlacklabelDbContext _context;

    public ScanRepository(BlacklabelDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Scan> Items, int TotalCount)> GetPagedByUserAsync(
        Guid userId, int page, int pageSize, CancellationToken ct)
    {
        var query = _context.Scans.Where(s => s.UserId == userId).OrderByDescending(s => s.ScannedAt);

        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return (items, totalCount);
    }

    public Task<int> CountSinceAsync(Guid userId, DateTime sinceUtc, CancellationToken ct)
        => _context.Scans.CountAsync(s => s.UserId == userId && s.ScannedAt >= sinceUtc, ct);

    public Task AddAsync(Scan scan, CancellationToken ct)
    {
        _context.Scans.Add(scan);
        return Task.CompletedTask;
    }

    public Task AddRangeAsync(IEnumerable<Scan> scans, CancellationToken ct)
    {
        _context.Scans.AddRange(scans);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _context.SaveChangesAsync(ct);
}
