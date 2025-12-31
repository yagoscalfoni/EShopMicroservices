using BuildingBlocks.CQRS;
using User.Application.Dtos;

namespace User.Application.Users.Queries.GetSupportTickets;

public record GetSupportTicketsQuery(Guid UserId) : IQuery<GetSupportTicketsResult>;

public record GetSupportTicketsResult(IReadOnlyCollection<SupportTicketDto> SupportTickets);
