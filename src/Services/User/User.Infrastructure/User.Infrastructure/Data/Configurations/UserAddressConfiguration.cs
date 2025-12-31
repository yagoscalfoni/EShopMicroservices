using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;

namespace User.Infrastructure.Data.Configurations
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedNever();

            builder.Property(a => a.UserId)
                .HasConversion(id => id.Value, value => UserId.Of(value))
                .IsRequired();

            builder.Property(a => a.Label)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Receiver)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.State)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.ZipCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.DeliveryNotes)
                .HasMaxLength(300);

            builder.ToTable("UserAddresses");
        }
    }
}
