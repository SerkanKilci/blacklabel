using Blacklabel.Application.Interfaces;
using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blacklabel.Infrastructure.Repositories;

public class HouseholdProfileRepository : IHouseholdProfileRepository
{
    private readonly BlacklabelDbContext _context;

    public HouseholdProfileRepository(BlacklabelDbContext context)
    {
        _context = context;
    }

    public Task<List<HouseholdProfile>> GetByUserIdAsync(Guid userId, CancellationToken ct)
        => _context.HouseholdProfiles.Where(p => p.UserId == userId).OrderBy(p => p.CreatedAt).ToListAsync(ct);

    public Task<HouseholdProfile?> GetByIdAsync(Guid profileId, Guid userId, CancellationToken ct)
        => _context.HouseholdProfiles.FirstOrDefaultAsync(p => p.Id == profileId && p.UserId == userId, ct);

    public Task AddAsync(HouseholdProfile profile, CancellationToken ct)
    {
        _context.HouseholdProfiles.Add(profile);
        return Task.CompletedTask;
    }

    public void Remove(HouseholdProfile profile) => _context.HouseholdProfiles.Remove(profile);

    public Task SaveChangesAsync(CancellationToken ct) => _context.SaveChangesAsync(ct);
}
