using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Interfaces;

public interface IAppUserRepository
{
    Task<AppUser?> GetByDeviceIdAsync(string deviceId, CancellationToken ct);
    Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(AppUser user, CancellationToken ct);
    Task RemoveAsync(AppUser user, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
