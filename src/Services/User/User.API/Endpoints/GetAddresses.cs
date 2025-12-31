using User.Application.Dtos;
using User.Application.Users.Queries.GetAddresses;

namespace User.API.Endpoints;

public record AddressListResponse(IReadOnlyCollection<AddressSummaryDto> Addresses);

public class GetAddresses : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/account/addresses/{userId:guid}", async (Guid userId, ISender sender) =>
        {
            var result = await sender.Send(new GetAddressesQuery(userId));
            return Results.Ok(new AddressListResponse(result.Addresses));
        })
        .WithName("GetAddresses")
        .Produces<AddressListResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Obtém endereços do cliente")
        .WithDescription("Retorna somente os endereços necessários para o menu de endereços.");
    }
}
