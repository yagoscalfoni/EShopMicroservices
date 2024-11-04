using User.Application.Users.Commands.AuthenticateUser;

namespace User.API.Endpoints;

/// <summary>
/// Requisição para autenticação de usuário.
/// </summary>
/// <param name="Email">Email do usuário.</param>
/// <param name="Password">Senha do usuário.</param>
public record AuthenticateUserRequest(string Email, string Password);

/// <summary>
/// Resposta para autenticação do usuário.
/// </summary>
/// <param name="Token">Token JWT gerado.</param>
/// <param name="ExpiresAt">Data de expiração do token.</param>
public record AuthenticateUserResponse(string Token, DateTime ExpiresAt, string Name);

public class AuthenticateUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/authenticate", async (AuthenticateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<AuthenticateUserCommand>();
            var result = await sender.Send(command);

            if (result == null)
            {
                return Results.Unauthorized();
            }

            var response = new AuthenticateUserResponse(result.Token, result.ExpiresAt, result.Name);

            return Results.Ok(response);
        })
        .WithName("AuthenticateUser")
        .Produces<AuthenticateUserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithSummary("Authenticate User")
        .WithDescription("Authenticates a user and returns a JWT token if credentials are valid.");
    }
}
