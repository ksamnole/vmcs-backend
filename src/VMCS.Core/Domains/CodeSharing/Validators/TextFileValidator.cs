using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCS.Core.Domains.CodeSharing.Models;

namespace VMCS.Core.Domains.CodeSharing.Validators
{
    internal class TextFileValidator : AbstractValidator<TextFile>
    {
        public TextFileValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Please specify a name")
                .Must((name) =>
                {
                    return char.IsLetterOrDigit(name[0]) &&
                    char.IsLetterOrDigit(name[name.Length - 1]) &&
                    name.Where(x => x == '.').Count() == 1 &&
                    !name.Where(x => !(char.IsLetterOrDigit(x) || x == '.')).Any();
                }).WithMessage("Invalid name for file");
        }
    }
}
