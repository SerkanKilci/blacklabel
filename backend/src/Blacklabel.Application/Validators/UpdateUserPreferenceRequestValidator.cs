using Blacklabel.Application.Dtos;
using Blacklabel.Application.Interfaces;
using FluentValidation;

namespace Blacklabel.Application.Validators;

public class UpdateUserPreferenceRequestValidator : AbstractValidator<UpdateUserPreferenceRequest>
{
    public UpdateUserPreferenceRequestValidator(IAdditiveRepository additiveRepository, IAllergenRepository allergenRepository)
    {
        RuleFor(x => x.AvoidedAdditiveCodes).NotNull();
        RuleForEach(x => x.AvoidedAdditiveCodes)
            .MustAsync(async (code, ct) => await additiveRepository.GetByCodeAsync(code, ct) is not null)
            .WithMessage("'{PropertyValue}' is not a known additive code.");

        RuleFor(x => x.AllergenCodes).NotNull();
        RuleForEach(x => x.AllergenCodes)
            .MustAsync(async (code, ct) => await allergenRepository.GetByCodeAsync(code, ct) is not null)
            .WithMessage("'{PropertyValue}' is not a known allergen code.");

        RuleFor(x => x.DietFlags).NotNull();
    }
}
