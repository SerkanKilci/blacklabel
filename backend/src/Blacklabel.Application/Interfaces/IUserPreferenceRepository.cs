using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Interfaces;

public interface IUserPreferenceRepository
{
    Task<UserPreference?> GetByUserIdAsync(Guid userId, CancellationToken ct);
    Task AddAsync(UserPreference preference, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
