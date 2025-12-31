using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;

namespace User.Infrastructure.Data.Configurations
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedNever();

            builder.Property(p => p.UserId)
                .HasConversion(id => id.Value, value => UserId.Of(value))
                .IsRequired();

            builder.Property(p => p.Brand)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Last4)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(p => p.Expiry)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.Type)
                .IsRequired()
                .HasMaxLength(50);

            builder.ToTable("PaymentMethods");
        }
    }
}
