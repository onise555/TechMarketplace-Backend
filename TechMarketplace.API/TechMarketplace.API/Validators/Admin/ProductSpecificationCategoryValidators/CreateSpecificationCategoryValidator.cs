using FluentValidation;
using TechMarketplace.API.Requests.Admin.AdminSpecificationCategoryRequests;

namespace TechMarketplace.API.Validators.Admin.ProductSpecificationCategoryValidators
{
    public class CreateSpecificationCategoryValidator : AbstractValidator<CreateSpecificationCategoryRequest>
    {
        public CreateSpecificationCategoryValidator() 
        {
            RuleFor(x => x.Name)
                       .NotEmpty().WithMessage("Specification Category Name is required.")
                       .MaximumLength(100).WithMessage("Specification Category Name cannot exceed 100 characters.");

            RuleFor(x => x.productDetailId)
                .GreaterThan(0).WithMessage("Product Detail Id must be greater than 0.");
        }
    }
}
