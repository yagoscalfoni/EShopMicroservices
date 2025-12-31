using BuildingBlocks.CQRS;
using FluentValidation;
using User.Application.Dtos;

namespace User.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid UserId, string FirstName, string LastName, string Email, string PhoneNumber)
    : ICommand<UpdateUserResult>;

public record UpdateUserResult(UserDto User);

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
