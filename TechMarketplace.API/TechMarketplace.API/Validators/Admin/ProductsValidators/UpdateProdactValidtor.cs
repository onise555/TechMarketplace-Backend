using FluentValidation;
using TechMarketplace.API.Data;
using TechMarketplace.API.Requests.Admin.AdminProductRequests;

namespace TechMarketplace.API.Validators.Admin.ProductsValidators
{
    public class UpdateProdactValidtor:AbstractValidator<UpdateProductRequest>
    {
        private readonly DataContext _data;
        public UpdateProdactValidtor(DataContext data)
        {
            _data = data;

            // Name
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters")
                .When(p => !string.IsNullOrEmpty(p.Name));

            // Model
            RuleFor(p => p.Model)
                .NotEmpty().WithMessage("Model is required")
                .MaximumLength(50).WithMessage("Model cannot exceed 50 characters")
                .Matches(@"^[a-zA-Z0-9\s\-]+$").WithMessage("Model can contain only letters, numbers, spaces and hyphens")
                .When(p => !string.IsNullOrEmpty(p.Model));

            // Price
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0")
                .When(p => p.Price != null);

            // SKU
            RuleFor(p => p.Sku)
                .NotEmpty().WithMessage("SKU is required")
                .When(p => !string.IsNullOrEmpty(p.Sku));

            // Status
            RuleFor(p => p.Status)
                .IsInEnum().WithMessage("Invalid Status value")
                .When(p => p.Status != null);

            // BrandId
            RuleFor(p => p.BrandId)
                .Must(ExistBrand).WithMessage("Brand does not exist")
                .When(p => p.BrandId != null);

            // SubCategoryId
            RuleFor(p => p.SubCategoryId)
                .Must(ExistSubCategory).WithMessage("SubCategory does not exist")
                .When(p => p.SubCategoryId != null);


            // File
            RuleFor(p => p.File)
                .Must(f => f.Length <= 5 * 1024 * 1024).WithMessage("File size must be less than 5 MB")
                .Must(f => f.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                           f.FileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                           f.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Only JPG, JPEG, and PNG files are allowed")
                .When(p => p.File != null);
        }

        private bool ExistBrand(int brandId)
        {
            return _data.Brands.Any(b => b.Id == brandId);
        }

        private bool ExistSubCategory(int subId)
        {
            return _data.SubCategories.Any(s => s.Id == subId);
        }
    }
}
