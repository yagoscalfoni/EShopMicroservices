using BuildingBlocks.CQRS;
using User.Application.Dtos;

namespace User.Application.Users.Queries.GetAccountJourney;

public record GetAccountJourneyQuery(Guid UserId) : IQuery<GetAccountJourneyResult>;

public record GetAccountJourneyResult(AccountJourneyDto Journey);
