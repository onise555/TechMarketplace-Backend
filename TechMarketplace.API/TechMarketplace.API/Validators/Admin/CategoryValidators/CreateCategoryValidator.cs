using FluentValidation;
using TechMarketplace.API.Requests.Admin.AdminCategoryRequests;

namespace TechMarketplace.API.Validators.Admin.CategoryValidators
{
    public class CreateCategoryValidator:AbstractValidator<CreateCatrgoryRequest>
    {
        public CreateCategoryValidator() 
        {
            RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Name is required.")
           .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
           .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Description)
           .NotEmpty().WithMessage("Description is required.")
           .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.File)
           .Must(f => f.Length > 0).WithMessage("File cannot be empty.")
           .Must(f => f.ContentType == "image/jpeg" || f.ContentType == "image/png")
           .WithMessage("Only JPEG or PNG images are allowed.")
           .Must(f => f.Length <= 2 * 1024 * 1024)
           .WithMessage("File size cannot exceed 2MB.")
           .When(x => x.File != null);
        }    
    }
}
