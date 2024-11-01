using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints;

/// <summary>
/// Requisição para criar um pedido.
/// </summary>
/// <param name="Order">Dados do pedido.</param>
public record CreateOrderRequest(OrderDto Order);

/// <summary>
/// Resposta para o pedido criado.
/// </summary>
/// <param name="Id">ID do pedido criado.</param>
public record CreateOrderResponse(Guid Id);

public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateOrderCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateOrderResponse>();

            return Results.Created($"/orders/{response.Id}", response);
        })
        .WithName("CreateOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create a new order")
        .WithDescription("Creates a new order based on the provided details in the request object.");
    }
}
