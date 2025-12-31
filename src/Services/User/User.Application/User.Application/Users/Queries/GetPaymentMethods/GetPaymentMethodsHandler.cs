using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;
using User.Application.Dtos;
using User.Application.Exceptions;
using User.Domain.ValueObjects;

namespace User.Application.Users.Queries.GetPaymentMethods;

public class GetPaymentMethodsHandler : IQueryHandler<GetPaymentMethodsQuery, GetPaymentMethodsResult>
{
    private readonly IApplicationDbContext _dbContext;

    public GetPaymentMethodsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetPaymentMethodsResult> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        var userExists = await _dbContext.Users.AnyAsync(u => u.Id.Value == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var methods = await _dbContext.PaymentMethods
            .AsNoTracking()
            .Where(o => o.UserId == UserId.Of(request.UserId))
            .OrderByDescending(p => p.Preferred)
            .Select(p => new PaymentMethodDto(
                p.Brand,
                p.Last4,
                p.Expiry,
                p.Preferred,
                p.Type))
            .ToListAsync(cancellationToken);

        return new GetPaymentMethodsResult(methods);
    }
}
