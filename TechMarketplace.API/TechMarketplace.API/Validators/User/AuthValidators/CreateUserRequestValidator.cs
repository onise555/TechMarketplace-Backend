using FluentValidation;
using System.Data;
using TechMarketplace.API.Requests.User.AuthRequests;

namespace TechMarketplace.API.Validators.User.AuthValidators
{
    public class CreateUserRequestValidator:AbstractValidator<CreateUserRequest>
    {

       public CreateUserRequestValidator() 
        {

            RuleFor(u => u.FirstName)
           .NotEmpty().WithMessage("First name is required")
           .Length(2, 50).WithMessage("First name must be between 2 and 50 characters")
           .Must(name => name.Trim().Length >= 2).WithMessage("First name must be at least 2 characters");

            RuleFor(u => u.LastName)
           .NotEmpty().WithMessage("Last name is required")
           .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters");

            RuleFor(u => u.Email)
           .NotEmpty().WithMessage("Email name is required")
           .EmailAddress().WithMessage("Invalid email format");

            RuleFor(u => u.Password)
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
