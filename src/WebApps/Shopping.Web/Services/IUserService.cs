namespace Shopping.Web.Services
{
    public interface IUserService
    {
        [Post("/user-service/authenticate")]
        Task<AuthenticateUserResponse> AuthenticateUser([Body] AuthenticateUserRequest request);
    }

    public record AuthenticateUserRequest(string Email, string Password);
    public record AuthenticateUserResponse(string Token, string Name);
}
