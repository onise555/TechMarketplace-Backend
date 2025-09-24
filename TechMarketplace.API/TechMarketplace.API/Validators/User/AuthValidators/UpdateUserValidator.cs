using FluentValidation;
using TechMarketplace.API.Requests.User.AuthRequests;

namespace TechMarketplace.API.Validators.User.AuthValidators
{
    public class UpdateUserValidator:AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator() 
        {
            // First Name
            RuleFor(x => x.FirstName)
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            // Last Name
            RuleFor(x => x.LastName)        
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            // Email
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .When(x => !string.IsNullOrEmpty(x.Email));



        }
    }
}
