using FluentValidation;
using TechMarketplace.API.Data;
using TechMarketplace.API.Requests.Admin.AdminSubCategoryRequest;

namespace TechMarketplace.API.Validators.Admin.SubCategoryValidators
{
    public class CreateSubCategoryValidator:AbstractValidator<CreateSubCategory>
    {

        private readonly DataContext _data;

        public CreateSubCategoryValidator(DataContext data) 
        {
            _data = data;

            RuleFor(x => x.Name)
             .NotEmpty().WithMessage("Name is required.")
             .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");


            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");


            RuleFor(p => p.CategoryId)
           .Must(ExistCategory)
           .WithMessage("SubCategory does not exist");
        }



        private bool ExistCategory(int CategoryId)
        {
            return _data.SubCategories.Any(x=>x.Id == CategoryId);
        }
    }
}
