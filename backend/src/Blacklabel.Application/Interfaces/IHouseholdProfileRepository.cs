using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Interfaces;

public interface IHouseholdProfileRepository
{
    Task<List<HouseholdProfile>> GetByUserIdAsync(Guid userId, CancellationToken ct);
    Task<HouseholdProfile?> GetByIdAsync(Guid profileId, Guid userId, CancellationToken ct);
    Task AddAsync(HouseholdProfile profile, CancellationToken ct);
    void Remove(HouseholdProfile profile);
    Task SaveChangesAsync(CancellationToken ct);
}
