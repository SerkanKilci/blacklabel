using Blacklabel.Domain.Entities;
using Blacklabel.Domain.Enums;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blacklabel.Tests.Repositories;

public class AppUserRepositoryTests
{
    private static BlacklabelDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<BlacklabelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BlacklabelDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task RemoveAsync_Cascades_To_Preferences_Scans_And_Contributions()
    {
        using var context = CreateContext();
        var repository = new AppUserRepository(context);

        var userId = Guid.NewGuid();
        var user = new AppUser { Id = userId, DeviceId = "device-to-delete", CreatedAt = DateTime.UtcNow };
        await repository.AddAsync(user, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        context.UserPreferences.Add(new UserPreference { UserId = userId });
        context.Scans.Add(new Scan { Id = Guid.NewGuid(), UserId = userId, Barcode = "8690504010104", ScannedAt = DateTime.UtcNow });
        context.Contributions.Add(new Contribution
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Barcode = "8690504010104",
            Status = ContributionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var trackedUser = await repository.GetByIdAsync(userId, CancellationToken.None);
        await repository.RemoveAsync(trackedUser!, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        Assert.Null(await context.AppUsers.FirstOrDefaultAsync(u => u.Id == userId));
        Assert.Null(await context.UserPreferences.FirstOrDefaultAsync(p => p.UserId == userId));
        Assert.Empty(await context.Scans.Where(s => s.UserId == userId).ToListAsync());
        Assert.Empty(await context.Contributions.Where(c => c.UserId == userId).ToListAsync());
    }

    [Fact]
    public async Task RemoveAsync_Frees_DeviceId_For_A_New_Anonymous_Account()
    {
        using var context = CreateContext();
        var repository = new AppUserRepository(context);

        var userId = Guid.NewGuid();
        await repository.AddAsync(new AppUser { Id = userId, DeviceId = "device-1", CreatedAt = DateTime.UtcNow }, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        var user = await repository.GetByIdAsync(userId, CancellationToken.None);
        await repository.RemoveAsync(user!, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        Assert.Null(await repository.GetByDeviceIdAsync("device-1", CancellationToken.None));
    }
}
