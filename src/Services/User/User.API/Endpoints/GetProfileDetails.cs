using User.Application.Dtos;
using User.Application.Users.Queries.GetProfileDetails;

namespace User.API.Endpoints;

public record ProfileDetailsResponse(ProfileDetailsDto Profile);

public class GetProfileDetails : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/account/profile/{userId:guid}", async (Guid userId, ISender sender) =>
        {
            var result = await sender.Send(new GetProfileDetailsQuery(userId));
            return Results.Ok(new ProfileDetailsResponse(result.Profile));
        })
        .WithName("GetProfileDetails")
        .Produces<ProfileDetailsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Obtém dados de perfil do cliente")
        .WithDescription("Retorna somente as informações utilizadas pelo menu de perfil.");
    }
}
