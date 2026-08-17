using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Interfaces;

public interface IAllergenRepository
{
    Task<Allergen?> GetByCodeAsync(string code, CancellationToken ct);
}
