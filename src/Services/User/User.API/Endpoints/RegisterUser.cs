using User.Application.Users.Commands.RegisterUser;

namespace User.API.Endpoints;

/// <summary>
/// Request to register a new user.
/// </summary>
/// <param name="FirstName">First name of the user.</param>
/// <param name="LastName">Last name of the user.</param>
/// <param name="Email">Email address.</param>
/// <param name="Password">User password.</param>
/// <param name="PhoneNumber">User phone number.</param>
public record RegisterUserRequest(string FirstName, string LastName, string Email, string Password, string PhoneNumber = "");

/// <summary>
/// Response containing created user information.
/// </summary>
/// <param name="Id">Id of the new user.</param>
/// <param name="Email">Email of the new user.</param>
public record RegisterUserResponse(Guid Id, string Email);

public class RegisterUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (RegisterUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterUserCommand>();
            var result = await sender.Send(command);
            var response = new RegisterUserResponse(result.Id, result.Email);
            return Results.Created($"/users/{response.Id}", response);
        })
        .WithName("RegisterUser")
        .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Register User")
        .WithDescription("Creates a new user account.");
    }
}
