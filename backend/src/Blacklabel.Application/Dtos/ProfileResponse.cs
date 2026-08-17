namespace Blacklabel.Application.Dtos;

public sealed record ProfileResponse(string? Email, bool HasAppleLink, bool HasGoogleLink);
