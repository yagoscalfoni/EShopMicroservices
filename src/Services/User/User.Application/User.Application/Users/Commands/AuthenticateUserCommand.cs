using BuildingBlocks.CQRS;
using FluentValidation;

namespace User.Application.Users.Commands.AuthenticateUser
{
    public record AuthenticateUserCommand(string Email, string Password) : ICommand<AuthenticateUserResult>;

    public record AuthenticateUserResult(string Token, DateTime ExpiresAt, string Name, Guid UserId);

    public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid email format");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
