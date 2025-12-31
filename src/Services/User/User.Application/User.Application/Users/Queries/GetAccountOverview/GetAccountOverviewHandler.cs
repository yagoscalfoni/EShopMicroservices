using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;
using User.Application.Dtos;
using User.Application.Exceptions;

namespace User.Application.Users.Queries.GetAccountOverview;

public class GetAccountOverviewHandler : IQueryHandler<GetAccountOverviewQuery, GetAccountOverviewResult>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAccountOverviewHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetAccountOverviewResult> Handle(GetAccountOverviewQuery request, CancellationToken cancellationToken)
    {
        var overview = await _dbContext.AccountOverviews
            .AsNoTracking()
            .Include(o => o.Benefits)
            .Include(o => o.PendingActions)
            .FirstOrDefaultAsync(o => o.UserId.Value == request.UserId, cancellationToken);

        if (overview is null)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Id.Value == request.UserId, cancellationToken);
            if (!userExists)
            {
                throw new UserNotFoundException(request.UserId);
            }

            return new GetAccountOverviewResult(
                new AccountOverviewDto(
                    string.Empty,
                    string.Empty,
                    new List<string>(),
                    string.Empty,
                    0,
                    new List<string>()
                )
            );
        }

        var overviewDto = new AccountOverviewDto(
            overview.NextDeliveryWindow,
            overview.LoyaltyLevel,
            overview.Benefits.Select(b => b.Description).ToList(),
            overview.LastOrderId,
            overview.LastOrderTotal,
            overview.PendingActions.Select(p => p.Description).ToList());

        return new GetAccountOverviewResult(overviewDto);
    }
}
