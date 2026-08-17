using Blacklabel.Application.Dtos;
using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Mapping;

public static class AdditiveMapper
{
    public static AdditiveResponse ToResponse(Additive additive) => new(
        additive.Code,
        additive.NameTr,
        additive.NameEn,
        additive.Category.ToString(),
        additive.RiskLevel,
        additive.DescriptionTr,
        additive.DescriptionEn,
        additive.SourceNote
    );
}
