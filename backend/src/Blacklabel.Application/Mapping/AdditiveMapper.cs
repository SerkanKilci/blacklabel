using Blacklabel.Application.Dtos;
using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Mapping;

public static class AdditiveMapper
{
    public static AdditiveResponse ToResponse(Additive additive) => new(
        additive.Code,
        additive.NameTr,
        additive.NameEn,
        additive.NameDe,
        additive.NameFr,
        additive.NameEs,
        additive.Category.ToString(),
        additive.RiskLevel,
        additive.DescriptionTr,
        additive.DescriptionEn,
        additive.DescriptionDe,
        additive.DescriptionFr,
        additive.DescriptionEs,
        additive.SourceNote
    );
}
