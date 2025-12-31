using BuildingBlocks.CQRS;
using User.Application.Dtos;

namespace User.Application.Users.Queries.GetAccountOverview;

public record GetAccountOverviewQuery(Guid UserId) : IQuery<GetAccountOverviewResult>;

public record GetAccountOverviewResult(AccountOverviewDto Overview);
