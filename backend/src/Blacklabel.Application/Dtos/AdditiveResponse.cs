namespace Blacklabel.Application.Dtos;

public record AdditiveResponse(
    string Code,
    string NameTr,
    string NameEn,
    string Category,
    int RiskLevel,
    string DescriptionTr,
    string DescriptionEn,
    string? SourceNote
);
