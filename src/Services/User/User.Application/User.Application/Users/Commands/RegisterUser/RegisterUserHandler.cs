using System.Security.Cryptography;
using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;
using User.Domain.Exceptions;
using User.Domain.Models;
using User.Domain.ValueObjects;

namespace User.Application.Users.Commands.RegisterUser;

public class RegisterUserHandler(IApplicationDbContext dbContext)
    : ICommandHandler<RegisterUserCommand, RegisterUserResult>
{
    public async Task<RegisterUserResult> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var exists = await dbContext.Users.AnyAsync(u => u.Email == command.Email, cancellationToken);
        if (exists)
        {
            throw new DomainException("Email already registered.");
        }

        var salt = GenerateSalt();
        var hash = HashPassword(command.Password, salt);

        var user = User.Domain.Models.User.Create(
            id: UserId.Of(Guid.NewGuid()),
            firstName: command.FirstName,
            lastName: command.LastName,
            email: command.Email,
            passwordHash: hash,
            passwordSalt: salt,
            createdAt: DateTime.UtcNow
        );

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterUserResult(user.Id.Value, user.Email);
    }

    private static string GenerateSalt()
    {
        var bytes = RandomNumberGenerator.GetBytes(16);
        return Convert.ToBase64String(bytes);
    }

    private static string HashPassword(string password, string salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
        return Convert.ToBase64String(pbkdf2.GetBytes(32));
    }
}
