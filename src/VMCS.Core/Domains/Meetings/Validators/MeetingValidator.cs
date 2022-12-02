using FluentValidation;

namespace VMCS.Core.Domains.Meetings.Validators;

public class MeetingValidator : AbstractValidator<Meeting>
{
    public MeetingValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Please specify a Name").Length(3, 20)
            .WithMessage("Name length from 3 to 20 characters");
    }
}