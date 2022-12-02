using FluentValidation;

namespace VMCS.Core.Domains.Channels.Validators;

public class ChannelValidator : AbstractValidator<Channel>
{
    public ChannelValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Please specify a Name").Length(3, 20);
    }
}