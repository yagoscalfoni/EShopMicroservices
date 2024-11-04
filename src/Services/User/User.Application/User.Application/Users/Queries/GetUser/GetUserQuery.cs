using BuildingBlocks.CQRS;
using User.Application.Dtos;

namespace User.Application.Users.Queries.GetUser
{
    public record GetUserQuery(Guid UserId) : IQuery<GetUserResult>;

    public record GetUserResult(UserDto User);
}
