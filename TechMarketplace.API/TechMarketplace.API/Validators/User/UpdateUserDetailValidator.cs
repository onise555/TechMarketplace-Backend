using FluentValidation;
using TechMarketplace.API.Requests.User;

namespace TechMarketplace.API.Validators.User
{
    public class UpdateUserDetailValidator:AbstractValidator<UpdateUserDetailRequest>
    {
        public UpdateUserDetailValidator() 
        {

            RuleFor(d => d.UserProfileImg)
                .Cascade(CascadeMode.Stop)
                .Must(IsValidUrl).WithMessage("Profile image must be a valid URL.")
                .When(d => !string.IsNullOrEmpty(d.UserProfileImg));

            RuleFor(d => d.DateOfBirth)
                .LessThan(DateTime.Today).WithMessage("Date of Birth cannot be in the future.")
                .When(d => d.DateOfBirth != null);

            RuleFor(d => d.PhoneNumber)
                .Matches(@"^\+?[0-9]{10,15}$")
                .WithMessage("Invalid phone number. Use digits only, optional +, 10-15 characters.")
                .When(d => !string.IsNullOrEmpty(d.PhoneNumber));


        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var validatedUri)
                   && (validatedUri.Scheme == Uri.UriSchemeHttp || validatedUri.Scheme == Uri.UriSchemeHttps);
        }
    }
    }
