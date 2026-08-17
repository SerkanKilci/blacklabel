using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(AppUser user);
}
