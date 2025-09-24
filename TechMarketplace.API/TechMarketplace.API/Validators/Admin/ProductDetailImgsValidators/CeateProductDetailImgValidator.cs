using FluentValidation;
using TechMarketplace.API.Requests.Admin.AdminDetailImgRequests;

namespace TechMarketplace.API.Validators.Admin.ProductDetailImgsValidators
{
    public class CeateProductDetailImgValidator : AbstractValidator<CreateDetailImgRequest>
    {
        public CeateProductDetailImgValidator() 
        {
            RuleFor(x => x.File)
           .NotNull()
           .WithMessage("Image file is required")
           .Must(BeValidImageFile)
           .WithMessage("File must be a valid image (jpg, jpeg, png, webp)")
           .Must(BeValidFileSize)
           .WithMessage("File size must be less than 5MB");

            RuleFor(x => x.ProductDetailId)
                .GreaterThan(0)
                .WithMessage("ProductDetailId must be greater than 0");
        }

        private bool BeValidImageFile(IFormFile file)
        {
            if (file == null) return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(fileExtension);
        }

        private bool BeValidFileSize(IFormFile file)
        {
            if (file == null) return false;

            // 5MB = 5 * 1024 * 1024 bytes
            return file.Length <= 5 * 1024 * 1024;
        }
    }
    }


