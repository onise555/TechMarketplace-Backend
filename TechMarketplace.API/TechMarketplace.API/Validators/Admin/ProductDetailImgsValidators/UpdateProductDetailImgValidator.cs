using FluentValidation;
using TechMarketplace.API.Requests.Admin.AdminDetailImgRequests;

namespace TechMarketplace.API.Validators.Admin.ProductDetailImgsValidators
{
    public class UpdateProductDetailImgValidator:AbstractValidator<UpdateDetailImgRequest>
    {
        public UpdateProductDetailImgValidator() 
        {
            RuleFor(x => x.File)
    .NotNull()
    .WithMessage("Image file is required")
    .Must(BeValidImageFile)
    .WithMessage("File must be a valid image (jpg, jpeg, png, webp)")
    .Must(BeValidFileSize)
    .WithMessage("File size must be less than 5MB");

           
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

            return file.Length <= 5 * 1024 * 1024; // 5MB
        }

    }
}
