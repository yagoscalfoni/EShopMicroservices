using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;
using User.Application.Dtos;
using User.Application.Exceptions;

namespace User.Application.Users.Queries.GetSupportTickets;

public class GetSupportTicketsHandler : IQueryHandler<GetSupportTicketsQuery, GetSupportTicketsResult>
{
    private readonly IApplicationDbContext _dbContext;

    public GetSupportTicketsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetSupportTicketsResult> Handle(GetSupportTicketsQuery request, CancellationToken cancellationToken)
    {
        var userExists = await _dbContext.Users.AnyAsync(u => u.Id.Value == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var tickets = await _dbContext.SupportTickets
            .AsNoTracking()
            .Where(r => r.UserId.Value == request.UserId)
            .OrderByDescending(t => t.UpdatedAt)
            .Select(t => new SupportTicketDto(
                t.TicketNumber,
                t.Subject,
                t.Status,
                t.UpdatedAt))
            .ToListAsync(cancellationToken);

        return new GetSupportTicketsResult(tickets);
    }
}
