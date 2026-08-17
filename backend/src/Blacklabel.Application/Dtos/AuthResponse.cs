namespace Blacklabel.Application.Dtos;

public sealed record AuthResponse(string Token, Guid UserId, bool IsPremium);
