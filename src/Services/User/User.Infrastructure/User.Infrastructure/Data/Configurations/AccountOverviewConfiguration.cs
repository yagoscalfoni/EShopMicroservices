using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;

namespace User.Infrastructure.Data.Configurations
{
    public class AccountOverviewConfiguration : IEntityTypeConfiguration<AccountOverview>
    {
        public void Configure(EntityTypeBuilder<AccountOverview> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .ValueGeneratedNever();

            builder.Property(o => o.UserId)
                .HasConversion(id => id.Value, value => UserId.Of(value))
                .IsRequired();

            builder.Property(o => o.NextDeliveryWindow)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(o => o.LoyaltyLevel)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.LastOrderId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.LastOrderTotal)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(o => o.Benefits)
                .WithOne()
                .HasForeignKey(b => b.OverviewId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.PendingActions)
                .WithOne()
                .HasForeignKey(p => p.OverviewId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("AccountOverviews");
        }
    }
}
