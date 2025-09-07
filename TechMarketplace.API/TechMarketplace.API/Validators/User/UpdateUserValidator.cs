using FluentValidation;
using TechMarketplace.API.Requests.User;

namespace TechMarketplace.API.Validators.User
{
    public class UpdateUserValidator:AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator() 
        {
      RuleFor(x => x.FirstName)
       .NotEmpty().WithMessage("First name is required")
       .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

           RuleFor(u => u.Password)
          .NotEmpty().WithMessage("Password is required")
          .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
          .MaximumLength(12).WithMessage("Password must not exceed 12 characters")
          .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
          .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
          .Matches("[0-9]").WithMessage("Password must contain at least one number")
          .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character")
          .When(x => !string.IsNullOrWhiteSpace(x.Password));
        }
    }
}
