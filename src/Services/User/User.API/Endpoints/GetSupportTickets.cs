using User.Application.Dtos;
using User.Application.Users.Queries.GetSupportTickets;

namespace User.API.Endpoints;

public record SupportTicketsResponse(IReadOnlyCollection<SupportTicketDto> Tickets);

public class GetSupportTickets : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/account/support/{userId:guid}", async (Guid userId, ISender sender) =>
        {
            var result = await sender.Send(new GetSupportTicketsQuery(userId));
            return Results.Ok(new SupportTicketsResponse(result.SupportTickets));
        })
        .WithName("GetSupportTickets")
        .Produces<SupportTicketsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Obtém chamados de suporte do cliente")
        .WithDescription("Retorna somente os chamados necessários para o menu de suporte.");
    }
}
