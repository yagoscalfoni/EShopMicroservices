using BuildingBlocks.CQRS;
using User.Application.Dtos;

namespace User.Application.Users.Queries.GetAddresses;

public record GetAddressesQuery(Guid UserId) : IQuery<GetAddressesResult>;

public record GetAddressesResult(IReadOnlyCollection<AddressSummaryDto> Addresses);
