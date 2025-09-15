namespace MmaSolution.Core.Validations
{
    public class SysSettingValidator : AbstractValidator<SysSettingModifyModel>
    {
        public SysSettingValidator()
        {

            RuleFor(e => e.SysKey).NotNull().WithMessage("Key_Required");
            RuleFor(e => e.SysValue).NotNull().WithMessage("Value_Required");
        }
    }
}
