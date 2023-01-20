using FluentValidation;
using VMCS.Core.Domains.CodeSharing.Models;

namespace VMCS.Core.Domains.CodeSharing.Validators;

public class FolderValidator : AbstractValidator<Folder>
{
    public FolderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Please specify a name")
            .Must(name => name != "" && !name.Where(x => !char.IsLetterOrDigit(x)).Any())
            .WithMessage("Invalid folder name");
    }
}