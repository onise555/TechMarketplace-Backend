using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization.DataContracts;
using TechMarketplace.API.Data;
using TechMarketplace.API.Models.Products;
using TechMarketplace.API.Requests.Admin.AdminProductRequests;

namespace TechMarketplace.API.Validators.Admin.ProductsValidators
{

    public class CreateProductValidator:AbstractValidator<CreateProductRequest>
    {

        private readonly DataContext _data;



        public CreateProductValidator(DataContext data)
        {
            _data = data;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(100);

            RuleFor(p => p.Model)
           .NotEmpty().WithMessage("Model is required")
           .MaximumLength(50).WithMessage("Model must be at most 50 characters")
           .Matches(@"^[a-zA-Z0-9\s\-]+$").WithMessage("Model can contain only letters, numbers, spaces and hyphens");


            RuleFor(p => p.CreatedAt)
                .LessThan(DateTime.UtcNow)
                .WithMessage("Created date must be in the past");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(p => p.Sku)
                .NotEmpty().WithMessage("SKU is required")
                .Must(CheckSku).WithMessage("SKU must be unique");

            RuleFor(p => p.Status)
           .IsInEnum().WithMessage("Invalid Status value");


            RuleFor(p => p.BrandId)
                .Must(ExistBrandId).WithMessage("Brand does not exist");


            RuleFor(p => p.SubCategoryId)
              .Must(SubCategoryExists)
              .WithMessage("SubCategory does not exist");


            RuleFor(p => p.File)
    .NotNull().WithMessage("Product image file is required")
    .Must(f => f.Length <= 5 * 1024 * 1024).WithMessage("File size must be less than 5 MB")
    .Must(f => f.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
               f.FileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
               f.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
    .WithMessage("Only JPG, JPEG, and PNG files are allowed");


        }


        private bool CheckSku(string sku)
        {
         return !_data.Products.Any(p=>p.Sku == sku);
        }

        private bool ExistBrandId(int brandId)
        {
        return _data.Brands.Any(b => b.Id == brandId);
        }

        private bool SubCategoryExists(int subId)
        {
        return _data.SubCategories.Any(s => s.Id == subId);
        }





    }
}
