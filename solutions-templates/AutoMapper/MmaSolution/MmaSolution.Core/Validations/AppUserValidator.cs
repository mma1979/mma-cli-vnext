namespace MmaSolution.Core.Validations
{
    public class AppUserValidator : AbstractValidator<AppUserModifyModel>
    {
        public AppUserValidator()
        {
            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email_Required")
                .EmailAddress().WithMessage("Valid_Email_Required");
            RuleFor(e => e.FullName)
                .NotEmpty().WithMessage("FullName_Required");
           RuleFor(e => e.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber_Required");
           

        }
    }
}
