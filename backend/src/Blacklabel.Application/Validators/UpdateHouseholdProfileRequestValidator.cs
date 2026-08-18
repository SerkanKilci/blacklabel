using Blacklabel.Application.Dtos;
using FluentValidation;

namespace Blacklabel.Application.Validators;

// Additive/allergen code existence is checked in MeController (async, against the repository)
// rather than here: ASP.NET's automatic FluentValidation pipeline is synchronous and cannot
// invoke MustAsync rules (throws AsyncValidatorInvokedSynchronouslyException at request time).
public class UpdateHouseholdProfileRequestValidator : AbstractValidator<UpdateHouseholdProfileRequest>
{
    public UpdateHouseholdProfileRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.AvoidedAdditiveCodes).NotNull();
        RuleFor(x => x.AllergenCodes).NotNull();
        RuleFor(x => x.DietFlags).NotNull();
    }
}

public class CreateHouseholdProfileRequestValidator : AbstractValidator<CreateHouseholdProfileRequest>
{
    public CreateHouseholdProfileRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}
