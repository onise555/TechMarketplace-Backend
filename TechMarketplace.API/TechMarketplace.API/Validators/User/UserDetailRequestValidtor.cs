using FluentValidation;
using TechMarketplace.API.Requests.User;

namespace TechMarketplace.API.Validators.User
{
    public class UserDetailRequestValidtor : AbstractValidator<CreateUserDetailRequest>
    {
        public UserDetailRequestValidtor()
        {
            RuleFor(d => d.UserProfileImg)
                .NotEmpty().WithMessage("Profile image is required.")
                .Must(IsValidUrl).WithMessage("Profile image must be a valid URL.");

            RuleFor(d => d.DateOfBirth)
           .NotEmpty().WithMessage("Date of Birth is required.")
           .LessThan(DateTime.Today).WithMessage("Date of Birth cannot be in the future.");

            RuleFor(d => d.PhoneNumber)
           .NotEmpty().WithMessage("Phone Number is required.")
           .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number. Use digits only, optional +, 10-15 characters.");



            RuleFor(a => a.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0");
        }

        private bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;

          
            if (url.StartsWith("/")) return true;

            return Uri.TryCreate(url, UriKind.Absolute, out var validatedUri)
                   && (validatedUri.Scheme == Uri.UriSchemeHttp || validatedUri.Scheme == Uri.UriSchemeHttps);
        }


    }
}
