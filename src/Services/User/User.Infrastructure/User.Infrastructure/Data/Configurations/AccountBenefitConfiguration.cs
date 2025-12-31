using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;

namespace User.Infrastructure.Data.Configurations
{
    public class AccountBenefitConfiguration : IEntityTypeConfiguration<AccountBenefit>
    {
        public void Configure(EntityTypeBuilder<AccountBenefit> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .ValueGeneratedNever();

            builder.Property(b => b.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.ToTable("AccountBenefits");
        }
    }
}
