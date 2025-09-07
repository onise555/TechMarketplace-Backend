using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace TechMarketplace.API.Validators.User
{
    public class ForgotPasswordValidator:AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordValidator() 
        {
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Email is required")
               .EmailAddress().WithMessage("Invalid email format");
        }
        
    }
}
