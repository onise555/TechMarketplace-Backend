using FluentValidation;
using TechMarketplace.API.Requests.Admin.AdminProductSpecificationRequests;

namespace TechMarketplace.API.Validators.Admin.ProductSpecificationValidators
{
    public class CreateProductSpecificationValidtor:AbstractValidator<SpecificationRequest>
    {
        public CreateProductSpecificationValidtor() 
        {

            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Specification Key is required.")
                .MaximumLength(100).WithMessage("Specification Key cannot exceed 100 characters.");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Specification Value is required.")
                .MaximumLength(500).WithMessage("Specification Value cannot exceed 500 characters.");

            RuleFor(x => x.SpecificationCategoryId)
                .GreaterThan(0).WithMessage("Specification Category Id must be greater than 0.");

        }

    }
}
