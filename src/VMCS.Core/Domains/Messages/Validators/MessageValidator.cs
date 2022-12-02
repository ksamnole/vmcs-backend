using FluentValidation;

namespace VMCS.Core.Domains.Messages.Validators;

public class MessageValidator : AbstractValidator<Message>
{
    public MessageValidator()
    {
        RuleFor(x => x.Text).NotEmpty()
            .WithMessage("Please specify a text of the message");
    }
}