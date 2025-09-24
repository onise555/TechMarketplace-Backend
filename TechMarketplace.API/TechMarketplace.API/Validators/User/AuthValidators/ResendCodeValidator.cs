using FluentValidation;
using TechMarketplace.API.Requests.User.AuthRequests;

namespace TechMarketplace.API.Validators.User.AuthValidators
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
