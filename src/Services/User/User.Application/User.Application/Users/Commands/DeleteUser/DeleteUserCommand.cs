using BuildingBlocks.CQRS;
using FluentValidation;

namespace User.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid UserId) : ICommand<DeleteUserResult>;

public record DeleteUserResult(bool Success);

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
