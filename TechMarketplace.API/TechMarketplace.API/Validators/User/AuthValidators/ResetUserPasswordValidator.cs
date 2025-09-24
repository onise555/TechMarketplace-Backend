using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace TechMarketplace.API.Validators.User.AuthValidators
{
    public class ResetUserPasswordValidator:AbstractValidator<ResetPasswordRequest>
    {
     
        public ResetUserPasswordValidator() 
        {

            RuleFor(x => x.Email)
             .NotEmpty().WithMessage("Email is required")
             .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.ResetCode)
             .NotEmpty().WithMessage("Verification code is required")
             .Length(6).WithMessage("Verification code must be 6 digits")
             .Matches(@"^\d{6}$").WithMessage("Verification code must contain only numbers");


            RuleFor(u => u.NewPassword)
           .NotEmpty().WithMessage("Password is required")
           .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
           .MaximumLength(12).WithMessage("Password must not exceed 12 characters")
           .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
           .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
           .Matches("[0-9]").WithMessage("Password must contain at least one number")
           .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
        }
    }
}
