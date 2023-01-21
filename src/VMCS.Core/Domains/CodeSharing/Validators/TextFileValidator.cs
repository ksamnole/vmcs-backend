using FluentValidation;
using VMCS.Core.Domains.CodeSharing.Models;

namespace VMCS.Core.Domains.CodeSharing.Validators;

internal class TextFileValidator : AbstractValidator<TextFile>
{
    public TextFileValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Please specify a name")
            .Must(name =>
            {
                return char.IsLetterOrDigit(name[0]) &&
                       char.IsLetterOrDigit(name[^1]) &&
                       name.Count(x => x == '.') == 1 && 
                       name.All(x => char.IsLetterOrDigit(x) || x == '.');
            }).WithMessage("Invalid name for file");
    }
}