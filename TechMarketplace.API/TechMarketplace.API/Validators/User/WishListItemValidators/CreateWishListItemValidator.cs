using FluentValidation;
using TechMarketplace.API.Requests.User.WishLisRequests;
using TechMarketplace.API.Requests.User.WishListItemRequests;

namespace TechMarketplace.API.Validators.User.WishListItemValidators
{
    public class CreateWishListItemValidator:AbstractValidator<CreateWishListItemRequest>
    {
        public CreateWishListItemValidator() 
        {
            RuleFor(x => x.WishListId)
           .GreaterThan(0)
           .WithMessage("WishListId must be greater than 0.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0.");
        }
    }
}
