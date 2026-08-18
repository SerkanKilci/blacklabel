namespace Blacklabel.Application.Dtos;

public sealed record ProfileWarningDto(Guid ProfileId, string ProfileName, IReadOnlyList<PersonalWarningDto> Warnings);
