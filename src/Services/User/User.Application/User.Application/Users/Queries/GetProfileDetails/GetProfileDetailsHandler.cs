using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;
using User.Application.Dtos;
using User.Application.Exceptions;

namespace User.Application.Users.Queries.GetProfileDetails;

public class GetProfileDetailsHandler : IQueryHandler<GetProfileDetailsQuery, GetProfileDetailsResult>
{
    private readonly IApplicationDbContext _dbContext;

    public GetProfileDetailsHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetProfileDetailsResult> Handle(GetProfileDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id.Value == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        var recommendations = await _dbContext.SecurityRecommendations
            .AsNoTracking()
            .Where(r => r.UserId.Value == request.UserId)
            .Select(r => r.Description)
            .ToListAsync(cancellationToken);

        var profile = new ProfileDetailsDto(
            $"{user.FirstName} {user.LastName}",
            user.Email,
            user.PhoneNumber,
            user.Document,
            user.MarketingOptIn,
            recommendations);

        return new GetProfileDetailsResult(profile);
    }
}
