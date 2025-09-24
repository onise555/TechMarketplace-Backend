using FluentValidation;
using TechMarketplace.API.Requests.User.CartItemRequests;

namespace TechMarketplace.API.Validators.User.CartItemValidators
{
    public class CreateCartItemValidator:AbstractValidator<CreateCartItemRequest>
    {
        public CreateCartItemValidator() 
        {
            RuleFor(x => x.ProductId)
                   .NotEmpty().WithMessage("ProductId is required.")
                   .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.CartId)
                .NotEmpty().WithMessage("CartId is required.")
                .GreaterThan(0).WithMessage("CartId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
