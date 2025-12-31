using BuildingBlocks.CQRS;
using User.Application.Dtos;

namespace User.Application.Users.Queries.GetUsers;

public record GetUsersQuery : IQuery<GetUsersResult>;

public record GetUsersResult(IEnumerable<UserDto> Users);
