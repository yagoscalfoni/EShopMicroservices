using BuildingBlocks.CQRS;
using User.Application.Dtos;

namespace User.Application.Users.Queries.GetProfileDetails;

public record GetProfileDetailsQuery(Guid UserId) : IQuery<GetProfileDetailsResult>;

public record GetProfileDetailsResult(ProfileDetailsDto Profile);
