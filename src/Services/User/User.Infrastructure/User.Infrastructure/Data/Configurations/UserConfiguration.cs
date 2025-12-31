using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User.Domain.Models.User>
    {
        public void Configure(EntityTypeBuilder<User.Domain.Models.User> builder)
        {
            // Configuração da chave primária
            builder.HasKey(u => u.Id);

            // Configuração para o UserId (Valor da chave primária)
            builder.Property(u => u.Id)
                   .HasConversion(
                       id => id.Value,           // Armazena apenas o valor GUID
                       value => UserId.Of(value) // Converte para UserId ao carregar
                   )
                   .IsRequired();

            // Configurações para campos básicos
            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.PhoneNumber)
                   .HasMaxLength(20);

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.Property(u => u.PasswordSalt)
                   .IsRequired();

            builder.Property(u => u.IsEmailVerified)
                   .IsRequired();

            builder.Property(u => u.IsPhoneNumberVerified)
                   .IsRequired();

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            builder.Property(u => u.LastLogin);

            builder.Property(u => u.FailedLoginAttempts)
                   .IsRequired();

            builder.Property(u => u.LockoutEnd);

            builder.Property(u => u.DateOfBirth);

            builder.Property(u => u.Gender)
                   .HasMaxLength(50);

            builder.Property(u => u.ProfilePictureUrl)
                   .HasMaxLength(500);

            builder.Property(u => u.PreferredLanguage)
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(u => u.MarketingOptIn)
                   .IsRequired();

            builder.Property(u => u.TwoFactorEnabled)
                   .IsRequired();

            builder.Property(u => u.Document)
                   .HasMaxLength(30);

            // Configurações para autenticação externa
            builder.Property(u => u.GoogleId)
                   .HasMaxLength(100);

            builder.Property(u => u.AppleId)
                   .HasMaxLength(100);

            builder.Property(u => u.FacebookId)
                   .HasMaxLength(100);

            // Relacionamentos de endereço (opcional, caso sejam chaves estrangeiras)
            builder.Property(u => u.BillingAddressId);
            builder.Property(u => u.ShippingAddressId);

            // Definindo o nome da tabela, se necessário
            builder.ToTable("Users");
        }
    }
}
