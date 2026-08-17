using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Interfaces;

public interface IAdditiveRepository
{
    Task<IReadOnlyList<Additive>> GetAllAsync(CancellationToken ct);
    Task<Additive?> GetByCodeAsync(string code, CancellationToken ct);
}
