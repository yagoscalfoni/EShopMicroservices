namespace Shopping.Web.Models.Register;

public class RegisterUserModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public record RegisterUserRequest(string FirstName, string LastName, string Email, string Password);
public record RegisterUserResponse(Guid Id, string Email);
