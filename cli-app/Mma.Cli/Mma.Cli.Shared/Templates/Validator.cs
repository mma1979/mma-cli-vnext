namespace Mma.Cli.Shared.Templates;

internal sealed class Validator
{
    public const string Template = @"using FluentValidation;

using $SolutionName.Core.Database.Tables;

namespace $SolutionName.Core.Validations
{
    public class $EntityNameValidator:AbstractValidator<$EntityNameModifyModel>
    {

        public $EntityNameValidator()
        {
           
        }


    }
}";
}
