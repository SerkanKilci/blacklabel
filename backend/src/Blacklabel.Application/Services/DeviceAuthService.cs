using Blacklabel.Application.Dtos;
using Blacklabel.Application.Interfaces;
using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Services;

public class DeviceAuthService : IDeviceAuthService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly ITokenService _tokenService;

    public DeviceAuthService(IAppUserRepository appUserRepository, ITokenService tokenService)
    {
        _appUserRepository = appUserRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> AuthenticateAsync(string deviceId, CancellationToken ct)
    {
        var user = await _appUserRepository.GetByDeviceIdAsync(deviceId, ct);
        if (user is null)
        {
            user = new AppUser
            {
                Id = Guid.NewGuid(),
                DeviceId = deviceId,
                IsPremium = false,
                CreatedAt = DateTime.UtcNow
            };

            await _appUserRepository.AddAsync(user, ct);
            await _appUserRepository.SaveChangesAsync(ct);
        }

        var token = _tokenService.GenerateToken(user);
        return new AuthResponse(token, user.Id, user.IsPremium);
    }
}
