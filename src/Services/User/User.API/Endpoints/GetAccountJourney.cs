using User.Application.Dtos;
using User.Application.Users.Queries.GetAccountJourney;

namespace User.API.Endpoints;

public record AccountJourneyResponse(AccountJourneyDto Journey);

public class GetAccountJourney : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/account/journey/{userId:guid}", async (Guid userId, ISender sender) =>
        {
            var result = await sender.Send(new GetAccountJourneyQuery(userId));
            return Results.Ok(new AccountJourneyResponse(result.Journey));
        })
        .WithName("GetAccountJourney")
        .Produces<AccountJourneyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Obtém jornada do cliente")
        .WithDescription("Retorna o resumo, perfil, endereços, pagamentos e suporte do cliente em tempo real.");
    }
}
