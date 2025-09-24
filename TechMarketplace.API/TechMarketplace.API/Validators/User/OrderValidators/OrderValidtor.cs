using FluentValidation;
using TechMarketplace.API.Requests.User.OrderRequests;

namespace TechMarketplace.API.Validators.User.OrderValidators
{
    public class OrderValidtor:AbstractValidator<CreateOrderRequest>
    {
        public OrderValidtor()
        {
            RuleFor(x => x.AddressId)
           .NotEmpty().WithMessage("AddressId is required.")
           .GreaterThan(0).WithMessage("AddressId must be greater than 0.");

            RuleFor(x => x.Notes)
           .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
     
        }

    }
}
