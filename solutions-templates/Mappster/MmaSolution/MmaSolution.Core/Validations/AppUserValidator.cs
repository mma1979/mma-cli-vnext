using FluentValidation;

using MmaSolution.Core.Models.Identity;

namespace MmaSolution.Core.Validations
{
    public class AppUserValidator : AbstractValidator<AppUserModifyModel>
    {
        public AppUserValidator()
        {
            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email_Required")
                .EmailAddress().WithMessage("Valid_Email_Required");
            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage("FirstName_Required");
            RuleFor(e => e.LastName)
               .NotEmpty().WithMessage("LastName_Required");
           RuleFor(e => e.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber_Required");
            RuleFor(e => e.Mobile)
               .NotEmpty().WithMessage("Mobile_Required");
            RuleFor(e => e.CountryCode).NotEmpty().WithMessage("CountryCode_Required");
            //RuleFor(e => e.Password)
            //     .NotEmpty().WithMessage("Password_Required");

        }
    }
}
