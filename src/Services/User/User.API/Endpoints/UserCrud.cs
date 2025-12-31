using Mapster;
using User.Application.Users.Commands.DeleteUser;
using User.Application.Users.Commands.RegisterUser;
using User.Application.Users.Commands.UpdateUser;
using User.Application.Users.Queries.GetUser;
using User.Application.Users.Queries.GetUsers;
using User.Domain.Exceptions;

namespace User.API.Endpoints;

public record CreateUserRequest(string FirstName, string LastName, string Email, string Password, string PhoneNumber = "");
public record UpdateUserRequest(string FirstName, string LastName, string Email, string PhoneNumber = "");

public class UserCrud : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (ISender sender) =>
        {
            var result = await sender.Send(new GetUsersQuery());
            return Results.Ok(result.Users);
        })
        .WithName("GetUsers")
        .Produces(StatusCodes.Status200OK)
        .WithSummary("Listar usuários")
        .WithDescription("Retorna todos os usuários cadastrados.");

        app.MapGet("/users/{userId:guid}", async (Guid userId, ISender sender) =>
        {
            try
            {
                var result = await sender.Send(new GetUserQuery(userId));
                return Results.Ok(result.User);
            }
            catch (DomainException)
            {
                return Results.NotFound();
            }
        })
        .WithName("GetUserById")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Obter usuário")
        .WithDescription("Busca um usuário pelo seu identificador.");

        app.MapPost("/users", async (CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterUserCommand>();
            var result = await sender.Send(command);
            return Results.Created($"/users/{result.Id}", result.User);
        })
        .WithName("CreateUser")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Criar usuário")
        .WithDescription("Cria um novo usuário no sistema.");

        app.MapPut("/users/{userId:guid}", async (Guid userId, UpdateUserRequest request, ISender sender) =>
        {
            try
            {
                var command = new UpdateUserCommand(
                    userId,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.PhoneNumber ?? string.Empty);
                var result = await sender.Send(command);
                return Results.Ok(result.User);
            }
            catch (DomainException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateUser")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Atualizar usuário")
        .WithDescription("Atualiza os dados de um usuário existente.");

        app.MapDelete("/users/{userId:guid}", async (Guid userId, ISender sender) =>
        {
            try
            {
                await sender.Send(new DeleteUserCommand(userId));
                return Results.NoContent();
            }
            catch (DomainException)
            {
                return Results.NotFound();
            }
        })
        .WithName("DeleteUser")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Remover usuário")
        .WithDescription("Exclui um usuário do sistema.");
    }
}
