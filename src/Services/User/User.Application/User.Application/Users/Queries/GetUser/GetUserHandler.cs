using BuildingBlocks.CQRS;
using User.Application.Data;
using User.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using User.Application.Extensions;
using User.Domain.Exceptions;
using User.Domain.ValueObjects;

namespace User.Application.Users.Queries.GetUser
{
    public class GetUserHandler : IQueryHandler<GetUserQuery, GetUserResult>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetUserHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetUserResult> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == UserId.Of(query.UserId), cancellationToken);

            if (user is null)
            {
                throw new DomainException($"User with id {query.UserId} was not found.");
            }

            return new GetUserResult(user.ToUserDto());
        }
    }
}
