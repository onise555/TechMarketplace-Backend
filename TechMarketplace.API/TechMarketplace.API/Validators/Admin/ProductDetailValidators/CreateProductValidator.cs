using FluentValidation;
using TechMarketplace.API.Requests.Admin.AdminProductDetailRequests;

namespace TechMarketplace.API.Validators.Admin.ProductDetailValidators
{
    public class CreateProductValidator:AbstractValidator<CreateProductDetailRequest>
    {

        public CreateProductValidator()
        {
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock cannot be negative")
                .LessThanOrEqualTo(10000)
                .WithMessage("Stock cannot exceed 10,000 units");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MinimumLength(10)
                .WithMessage("Description must be at least 10 characters")
                .MaximumLength(1000)
                .WithMessage("Description cannot exceed 1000 characters")
                .Matches(@"^[a-zA-Z0-9\s.,!?-]+$")
                .WithMessage("Description contains invalid characters");

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0");
        }
    }
}
