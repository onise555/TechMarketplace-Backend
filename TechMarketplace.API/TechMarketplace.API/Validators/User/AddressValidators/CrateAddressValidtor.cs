using FluentValidation;
using TechMarketplace.API.Requests.User.AddressRequests;

namespace TechMarketplace.API.Validators.User.AddressValidators
{
    public class CrateAddressValidtor:AbstractValidator<CreateUserAddressRequest>
    {
        public CrateAddressValidtor()
        {
            RuleFor(a => a.Country).NotEmpty().WithMessage("Country is required");
            RuleFor(a => a.Street).NotEmpty().WithMessage("Street is required");
            RuleFor(a => a.City).NotEmpty().WithMessage("City is required");
            RuleFor(a => a.ZipCode)
                .NotEmpty().WithMessage("ZipCode is required")
                .Matches(@"^\d{5}(-\d{4})?$").WithMessage("Invalid ZipCode format");
            RuleFor(a => a.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0");
        }
    }
}
