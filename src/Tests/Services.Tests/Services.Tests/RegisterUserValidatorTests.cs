using User.Application.Users.Commands.RegisterUser;

namespace Services.Tests;

public class RegisterUserValidatorTests
{
    [Fact]
    public void Valid_command_passes()
    {
        var validator = new RegisterUserCommandValidator();
        var cmd = new RegisterUserCommand("John", "Doe", "john@example.com", "Strong1");
        var result = validator.Validate(cmd);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Invalid_email_fails()
    {
        var validator = new RegisterUserCommandValidator();
        var cmd = new RegisterUserCommand("John", "Doe", "bademail", "Strong1");
        var result = validator.Validate(cmd);
        Assert.False(result.IsValid);
    }
}
