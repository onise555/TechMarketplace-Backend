using FluentValidation;
using TechMarketplace.API.Requests.User.WishLisRequests;
using TechMarketplace.API.Requests.User.WishListItemRequests;

namespace TechMarketplace.API.Validators.User.WishListValidators
{
    public class CreateWishListValidator:AbstractValidator<CreateWishListRequest>
    {

        public CreateWishListValidator() 
        {
            RuleFor(x => x.UserId)
          .GreaterThan(0).WithMessage("User Id must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Wish List Name is required.")
                .MaximumLength(100).WithMessage("Wish List Name cannot exceed 100 characters.");
        }    

    }
}
