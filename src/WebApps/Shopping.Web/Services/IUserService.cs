namespace Shopping.Web.Services
{
    public interface IUserService
    {
        [Post("/user-service/authenticate")]
        Task<AuthenticateUserResponse> AuthenticateUser([Body] AuthenticateUserRequest request);

        [Post("/user-service/register")]
        Task<RegisterUserResponse> RegisterUser([Body] RegisterUserRequest request);
    }

    public record AuthenticateUserRequest(string Email, string Password);
    public record AuthenticateUserResponse(string Token, string Name, Guid UserId);
    public record RegisterUserRequest(string FirstName, string LastName, string Email, string Password);
    public record RegisterUserResponse(Guid Id, string Email);
}
