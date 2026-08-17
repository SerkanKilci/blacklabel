using Blacklabel.Application.Interfaces;
using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blacklabel.Infrastructure.Repositories;

public class UserPreferenceRepository : IUserPreferenceRepository
{
    private readonly BlacklabelDbContext _context;

    public UserPreferenceRepository(BlacklabelDbContext context)
    {
        _context = context;
    }

    public Task<UserPreference?> GetByUserIdAsync(Guid userId, CancellationToken ct)
        => _context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == userId, ct);

    public Task AddAsync(UserPreference preference, CancellationToken ct)
    {
        _context.UserPreferences.Add(preference);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _context.SaveChangesAsync(ct);
}
