namespace MmaSolution.Core.Validations
{
    public class RoleValidator : AbstractValidator<AppRoleModifyModel>
    {
        public RoleValidator()
        {
            RuleFor(e => e.Name).NotEmpty().WithMessage("Field_Required");
        }
    }
}
