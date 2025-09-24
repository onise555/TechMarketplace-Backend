using FluentValidation;
using TechMarketplace.API.Requests.User.CartRequests;

namespace TechMarketplace.API.Validators.User.CartValidators
{
    public class CreateCartValidator:AbstractValidator<CreateCartRequest>
    {
        public CreateCartValidator() 
        {
            RuleFor(x => x.UserId)
           .NotEmpty().WithMessage("UserId აუცილებელია.")
           .GreaterThan(0).WithMessage("UserId უნდა იყოს 1-ზე მეტი.");
        }    
    }
}
