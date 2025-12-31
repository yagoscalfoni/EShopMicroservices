using User.Application.Dtos;
using User.Application.Users.Queries.GetPaymentMethods;

namespace User.API.Endpoints;

public record PaymentMethodsResponse(IReadOnlyCollection<PaymentMethodDto> PaymentMethods);

public class GetPaymentMethods : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/account/payments/{userId:guid}", async (Guid userId, ISender sender) =>
        {
            var result = await sender.Send(new GetPaymentMethodsQuery(userId));
            return Results.Ok(new PaymentMethodsResponse(result.PaymentMethods));
        })
        .WithName("GetPaymentMethods")
        .Produces<PaymentMethodsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Obtém formas de pagamento do cliente")
        .WithDescription("Retorna somente as formas de pagamento necessárias para o menu de pagamentos.");
    }
}
