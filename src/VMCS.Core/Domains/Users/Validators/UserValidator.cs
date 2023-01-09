using FluentValidation;
using VMCS.Core.Domains.Users.Repositories;

namespace VMCS.Core.Domains.Users.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Please specify a Login")
            .Length(5, 20).WithMessage("Login length from 5 to 20 characters")
            .Must(login => !userRepository.ContainsByLogin(login))
                .WithMessage("Login is already in use");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Please specify a Username")
            .Length(5, 20).WithMessage("Username length from 5 to 20 characters");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please specify a Email")
            .EmailAddress().WithMessage("A valid email address is required");
    }
}