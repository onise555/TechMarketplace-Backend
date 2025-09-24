using FluentValidation;
using TechMarketplace.API.Requests.User.AddressRequests;

namespace TechMarketplace.API.Validators.User.AddressValidators
{
    public class UpdateUserAddressValidator:AbstractValidator<UpdateUserAddressRequest>
    {
        public UpdateUserAddressValidator() 
        {
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required");
            RuleFor(x => x.Street).NotEmpty().WithMessage("Street is required");
            RuleFor(x => x.ZipCode).NotEmpty().WithMessage("ZipCode is required");
            RuleFor(x => x.Label).NotEmpty().WithMessage("Label is required");
        }

    }
}
