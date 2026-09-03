using Blacklabel.Application.Interfaces;
using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blacklabel.Infrastructure.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly BlacklabelDbContext _context;

    public AppUserRepository(BlacklabelDbContext context)
    {
        _context = context;
    }

    public Task<AppUser?> GetByDeviceIdAsync(string deviceId, CancellationToken ct)
        => _context.AppUsers.FirstOrDefaultAsync(u => u.DeviceId == deviceId, ct);

    public Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct)
        => _context.AppUsers.FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<AppUser?> GetByAppleUserIdAsync(string appleUserId, CancellationToken ct)
        => _context.AppUsers.FirstOrDefaultAsync(u => u.AppleUserId == appleUserId, ct);

    public Task<AppUser?> GetByGoogleUserIdAsync(string googleUserId, CancellationToken ct)
        => _context.AppUsers.FirstOrDefaultAsync(u => u.GoogleUserId == googleUserId, ct);

    public Task AddAsync(AppUser user, CancellationToken ct)
    {
        _context.AppUsers.Add(user);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(AppUser user, CancellationToken ct)
    {
        _context.AppUsers.Remove(user);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
        => _context.SaveChangesAsync(ct);
}
