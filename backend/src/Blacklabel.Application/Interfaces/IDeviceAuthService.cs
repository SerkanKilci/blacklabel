using Blacklabel.Application.Dtos;

namespace Blacklabel.Application.Interfaces;

public interface IDeviceAuthService
{
    Task<AuthResponse> AuthenticateAsync(string deviceId, CancellationToken ct);
}
