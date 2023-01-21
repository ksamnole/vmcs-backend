using FluentValidation;
using VMCS.Core.Domains.CodeSharing.Models;

namespace VMCS.Core.Domains.CodeSharing.Validators;

public class FolderValidator : AbstractValidator<Folder>
{
    public FolderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Please specify a name")
            .Must(name => name != "" && name.All(char.IsLetterOrDigit))
            .WithMessage("Invalid folder name");
    }
}