using FluentValidation;
using TechMarketplace.API.Requests.User.CartItemRequests;

namespace TechMarketplace.API.Validators.User.CartItemValidators
{
    public class UpdateCartItemValidator:AbstractValidator<UpdateCartitemQuantityRequest>
    {
        public UpdateCartItemValidator()
        {
           RuleFor(x => x.Quantity)
          .NotEmpty().WithMessage("Quantity is required.")
          .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
