using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;
using User.Application.Dtos;
using User.Application.Exceptions;
using User.Domain.ValueObjects;

namespace User.Application.Users.Queries.GetAccountJourney;

public class GetAccountJourneyHandler : IQueryHandler<GetAccountJourneyQuery, GetAccountJourneyResult>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAccountJourneyHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetAccountJourneyResult> Handle(GetAccountJourneyQuery request, CancellationToken cancellationToken)
    {
        var userId = UserId.Of(request.UserId);

        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var overview = await _dbContext.AccountOverviews
            .AsNoTracking()
            .Include(o => o.Benefits)
            .Include(o => o.PendingActions)
            .FirstOrDefaultAsync(o => o.UserId == userId, cancellationToken);

        var addresses = await _dbContext.UserAddresses
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ToListAsync(cancellationToken);

        var paymentMethods = await _dbContext.PaymentMethods
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.Preferred)
            .ToListAsync(cancellationToken);

        var tickets = await _dbContext.SupportTickets
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.UpdatedAt)
            .ToListAsync(cancellationToken);

        var recommendations = await _dbContext.SecurityRecommendations
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .ToListAsync(cancellationToken);

        var journey = new AccountJourneyDto(
            new AccountOverviewDto(
                overview?.NextDeliveryWindow ?? string.Empty,
                overview?.LoyaltyLevel ?? string.Empty,
                overview?.Benefits.Select(b => b.Description).ToList() ?? new List<string>(),
                overview?.LastOrderId ?? string.Empty,
                overview?.LastOrderTotal ?? 0,
                overview?.PendingActions.Select(p => p.Description).ToList() ?? new List<string>()),
            new ProfileDetailsDto(
                $"{user.FirstName} {user.LastName}",
                user.Email,
                user.PhoneNumber,
                user.Document,
                user.MarketingOptIn,
                recommendations.Select(r => r.Description).ToList()),
            addresses.Select(a => new AddressSummaryDto(
                a.Label,
                a.Receiver,
                a.Street,
                a.City,
                a.State,
                a.ZipCode,
                a.IsDefault,
                a.DeliveryNotes)).ToList(),
            paymentMethods.Select(p => new PaymentMethodDto(
                p.Brand,
                p.Last4,
                p.Expiry,
                p.Preferred,
                p.Type)).ToList(),
            tickets.Select(t => new SupportTicketDto(
                t.TicketNumber,
                t.Subject,
                t.Status,
                t.UpdatedAt)).ToList());

        return new GetAccountJourneyResult(journey);
    }
}
