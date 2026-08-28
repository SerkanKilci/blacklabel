namespace Blacklabel.Application.Dtos;

public record AdditiveResponse(
    string Code,
    string NameTr,
    string NameEn,
    string NameDe,
    string NameFr,
    string NameEs,
    string Category,
    int RiskLevel,
    string DescriptionTr,
    string DescriptionEn,
    string DescriptionDe,
    string DescriptionFr,
    string DescriptionEs,
    string? SourceNote
);
