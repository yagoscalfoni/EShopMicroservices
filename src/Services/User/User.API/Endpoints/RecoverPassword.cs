using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace User.API.Endpoints;

/// <summary>
/// Request to initiate password recovery.
/// </summary>
/// <param name="Email">User email.</param>
public record RecoverPasswordRequest(string Email);

/// <summary>
/// Response for password recovery request.
/// </summary>
/// <param name="Message">Result message.</param>
public record RecoverPasswordResponse(string Message);

public class RecoverPassword : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/recover-password", (RecoverPasswordRequest request, ILogger<RecoverPassword> logger) =>
        {
            logger.LogInformation("Password recovery requested for {Email}", request.Email);
            return Results.Ok(new RecoverPasswordResponse("If the email exists, recovery instructions have been sent."));
        })
        .WithName("RecoverPassword")
        .Produces<RecoverPasswordResponse>(StatusCodes.Status200OK)
        .WithSummary("Recover Password")
        .WithDescription("Initiates password recovery for the given email.");
    }
}
