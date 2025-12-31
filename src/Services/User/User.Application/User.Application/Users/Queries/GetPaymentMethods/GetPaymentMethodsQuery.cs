using BuildingBlocks.CQRS;
using User.Application.Dtos;

namespace User.Application.Users.Queries.GetPaymentMethods;

public record GetPaymentMethodsQuery(Guid UserId) : IQuery<GetPaymentMethodsResult>;

public record GetPaymentMethodsResult(IReadOnlyCollection<PaymentMethodDto> PaymentMethods);
