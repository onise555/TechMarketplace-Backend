using FluentValidation;
using TechMarketplace.API.Requests.User;

namespace TechMarketplace.API.Validators.User
{
    public class CreateUserLoginValidator:AbstractValidator<CreateLoginRequest>
    {

        public CreateUserLoginValidator()
        {
           RuleFor(u=>u.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");


            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");

        }
    }
}
