using Blacklabel.Application.Interfaces;
using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blacklabel.Infrastructure.Repositories;

public class ContributionRepository : IContributionRepository
{
    private readonly BlacklabelDbContext _context;

    public ContributionRepository(BlacklabelDbContext context)
    {
        _context = context;
    }

    public Task<int> CountSinceAsync(Guid userId, DateTime sinceUtc, CancellationToken ct)
        => _context.Contributions.CountAsync(c => c.UserId == userId && c.CreatedAt >= sinceUtc, ct);

    public Task AddAsync(Contribution contribution, CancellationToken ct)
    {
        _context.Contributions.Add(contribution);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _context.SaveChangesAsync(ct);
}
