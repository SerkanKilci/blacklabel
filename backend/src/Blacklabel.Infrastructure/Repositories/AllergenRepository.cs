using Blacklabel.Application.Interfaces;
using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blacklabel.Infrastructure.Repositories;

public class AllergenRepository : IAllergenRepository
{
    private readonly BlacklabelDbContext _context;

    public AllergenRepository(BlacklabelDbContext context)
    {
        _context = context;
    }

    public Task<Allergen?> GetByCodeAsync(string code, CancellationToken ct)
        => _context.Allergens.AsNoTracking().FirstOrDefaultAsync(a => a.Code == code, ct);
}
