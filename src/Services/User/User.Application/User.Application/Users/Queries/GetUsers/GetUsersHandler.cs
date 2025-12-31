using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;
using User.Application.Extensions;

namespace User.Application.Users.Queries.GetUsers;

public class GetUsersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetUsersQuery, GetUsersResult>
{
    public async Task<GetUsersResult> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await dbContext.Users
            .AsNoTracking()
            .Select(u => u.ToUserDto())
            .ToListAsync(cancellationToken);

        return new GetUsersResult(users);
    }
}
