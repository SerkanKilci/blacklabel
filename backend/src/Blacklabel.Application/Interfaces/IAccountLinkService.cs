using Blacklabel.Application.Dtos;

namespace Blacklabel.Application.Interfaces;

public interface IAccountLinkService
{
    Task<AuthResponse?> LinkAsync(Guid userId, LinkAccountRequest request, CancellationToken ct);
}
