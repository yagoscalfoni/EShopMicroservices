using BuildingBlocks.CQRS;
using FluentValidation;
using User.Application.Dtos;

namespace User.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string FirstName, string LastName, string Email, string Password, string PhoneNumber = "") : ICommand<RegisterUserResult>;

public record RegisterUserResult(Guid Id, string Email, UserDto User);

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit");
    }
}
