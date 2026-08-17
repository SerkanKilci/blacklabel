using Blacklabel.Application.Services;
using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blacklabel.Tests.Services;

public class DeviceAuthServiceTests
{
    private class FakeTokenService : Blacklabel.Application.Interfaces.ITokenService
    {
        public string GenerateToken(AppUser user) => $"token-for-{user.Id}";
    }

    private static (BlacklabelDbContext Context, DeviceAuthService Service) CreateService()
    {
        var options = new DbContextOptionsBuilder<BlacklabelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BlacklabelDbContext(options);
        context.Database.EnsureCreated();

        var service = new DeviceAuthService(new AppUserRepository(context), new FakeTokenService());
        return (context, service);
    }

    [Fact]
    public async Task AuthenticateAsync_Creates_New_User_For_Unknown_Device()
    {
        var (context, service) = CreateService();

        var response = await service.AuthenticateAsync("device-1", CancellationToken.None);

        Assert.Equal($"token-for-{response.UserId}", response.Token);
        Assert.False(response.IsPremium);

        var user = await context.AppUsers.FirstAsync(u => u.DeviceId == "device-1");
        Assert.Equal(response.UserId, user.Id);
    }

    [Fact]
    public async Task AuthenticateAsync_Reuses_Existing_User_For_Known_Device()
    {
        var (context, service) = CreateService();

        var first = await service.AuthenticateAsync("device-1", CancellationToken.None);
        var second = await service.AuthenticateAsync("device-1", CancellationToken.None);

        Assert.Equal(first.UserId, second.UserId);
        Assert.Equal(1, await context.AppUsers.CountAsync());
    }
}
