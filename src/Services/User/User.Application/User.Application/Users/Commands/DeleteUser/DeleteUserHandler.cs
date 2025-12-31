using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;
using User.Domain.Exceptions;
using User.Domain.ValueObjects;

namespace User.Application.Users.Commands.DeleteUser;

public class DeleteUserHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteUserCommand, DeleteUserResult>
{
    public async Task<DeleteUserResult> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == UserId.Of(command.UserId), cancellationToken);
        if (user is null)
        {
            throw new DomainException($"User with id {command.UserId} was not found.");
        }

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteUserResult(true);
    }
}
