using FluentValidation;

namespace Taxually.TechnicalTest.Validators;

public class VatRegistrationRequestValidator : AbstractValidator<VatRegistrationRequest>
{
    public VatRegistrationRequestValidator()
    {
        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CompanyId).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Country).NotEmpty().Length(2);
    }
}
