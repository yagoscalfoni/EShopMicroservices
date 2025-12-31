using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;
using User.Application.Dtos;
using User.Application.Exceptions;
using User.Domain.ValueObjects;

namespace User.Application.Users.Queries.GetAddresses;

public class GetAddressesHandler : IQueryHandler<GetAddressesQuery, GetAddressesResult>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAddressesHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetAddressesResult> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        var userExists = await _dbContext.Users.AnyAsync(u => u.Id == UserId.Of(request.UserId), cancellationToken);
        if (!userExists)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var addresses = await _dbContext.UserAddresses
            .AsNoTracking()
            .Where(o => o.UserId == UserId.Of(request.UserId))
            .OrderByDescending(a => a.IsDefault)
            .Select(a => new AddressSummaryDto(
                a.Label,
                a.Receiver,
                a.Street,
                a.City,
                a.State,
                a.ZipCode,
                a.IsDefault,
                a.DeliveryNotes))
            .ToListAsync(cancellationToken);

        return new GetAddressesResult(addresses);
    }
}
