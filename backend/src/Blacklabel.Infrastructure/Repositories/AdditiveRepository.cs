using Blacklabel.Application.Interfaces;
using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blacklabel.Infrastructure.Repositories;

public class AdditiveRepository : IAdditiveRepository
{
    private readonly BlacklabelDbContext _context;

    public AdditiveRepository(BlacklabelDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Additive>> GetAllAsync(CancellationToken ct)
        => await _context.Additives.AsNoTracking().OrderBy(a => a.Code).ToListAsync(ct);

    public Task<Additive?> GetByCodeAsync(string code, CancellationToken ct)
        => _context.Additives.AsNoTracking().FirstOrDefaultAsync(a => a.Code == code, ct);
}
