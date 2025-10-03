using FluentValidation;
using TechMarketplace.API.Requests.Admin.AdminBrandRequests;

namespace TechMarketplace.API.Validators.Admin.BrandValidators
{
    public class CreateBrandValidators:AbstractValidator<CreateBrandRequest>
    {

        public CreateBrandValidators() 
        {
        
        

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");


            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");


            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.")
                .Must(f => f.Length > 0).WithMessage("File cannot be empty.")
                .Must(f => f.ContentType == "image/jpeg" || f.ContentType == "image/png")
                    .WithMessage("Only JPEG or PNG images are allowed.")
                .Must(f => f.Length <= 2 * 1024 * 1024)
                    .WithMessage("File size cannot exceed 2MB.");

    
        }
    }  
    }

