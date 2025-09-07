using FluentValidation;
using TechMarketplace.API.Requests.User;

namespace TechMarketplace.API.Validators.User
{
    public class VerifyUserValidator:AbstractValidator<VerifyRequest>
    {
        public VerifyUserValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Verification code is required")
                .Length(6).WithMessage("Verification code must be 6 digits")
                .Matches(@"^\d{6}$").WithMessage("Verification code must contain only numbers");
        }
    }
}
