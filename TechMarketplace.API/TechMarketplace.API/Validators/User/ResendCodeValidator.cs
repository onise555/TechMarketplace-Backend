using FluentValidation;
using TechMarketplace.API.Requests.User;

namespace TechMarketplace.API.Validators.User
{
    public class ResendCodeValidator:AbstractValidator<ResendCodeRequest>
    {
        public ResendCodeValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Email is required")
               .EmailAddress().WithMessage("Invalid email format");
        }
    }
}
