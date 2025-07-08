using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuildingBlocks.CQRS;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using User.Application.Data;

namespace User.Application.Users.Commands.AuthenticateUser
{
    public class AuthenticateUserHandler(IApplicationDbContext dbContext, IConfiguration configuration)
        : ICommandHandler<AuthenticateUserCommand, AuthenticateUserResult>
    {
        public async Task<AuthenticateUserResult> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken)
        {
            // Busca o usuário no banco de dados
            var user = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

            if (user == null || user.PasswordHash != HashPassword(command.Password)) // Verificação de hash da senha
            {
                return null; // Retorna null se o usuário não for encontrado ou a senha for inválida
            }

            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddHours(1);

            return new AuthenticateUserResult(token, expiresAt, user.FirstName + " " + user.LastName, user.Id.Value);
        }

        private string GenerateJwtToken(User.Domain.Models.User user)
        {
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("scope", "user.read user.purchase"),
                new Claim("name", $"{user.FirstName} {user.LastName}"),
                new Claim("userId", $"{user.Id.Value}")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password)
        {
            // Implementação de hash para validação de senha (mock)
            return password; // Substitua por uma função de hash real em produção
        }
    }
}
