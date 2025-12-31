using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;
using User.Application.Extensions;
using User.Application.Users.Commands.UpdateUser;
using User.Domain.Exceptions;

namespace User.Application.Users.Commands.UpdateUser;

public class UpdateUserHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id.Value == command.UserId, cancellationToken);
        if (user is null)
        {
            throw new DomainException($"User with id {command.UserId} was not found.");
        }

        var emailInUse = await dbContext.Users
            .AnyAsync(x => x.Email == command.Email && x.Id.Value != command.UserId, cancellationToken);

        if (emailInUse)
        {
            throw new DomainException("Email already registered.");
        }

        user.UpdateProfile(command.FirstName, command.LastName, command.Email, command.PhoneNumber);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateUserResult(user.ToUserDto());
    }
}
