using User.Application.Dtos;
using User.Application.Users.Queries.GetAccountOverview;

namespace User.API.Endpoints;

public record AccountOverviewResponse(AccountOverviewDto Overview);

public class GetAccountOverview : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/account/overview/{userId:guid}", async (Guid userId, ISender sender) =>
        {
            var result = await sender.Send(new GetAccountOverviewQuery(userId));
            return Results.Ok(new AccountOverviewResponse(result.Overview));
        })
        .WithName("GetAccountOverview")
        .Produces<AccountOverviewResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Obtém o resumo da conta do cliente")
        .WithDescription("Retorna apenas os dados necessários para o menu de resumo, minimizando carga e latência.");
    }
}
