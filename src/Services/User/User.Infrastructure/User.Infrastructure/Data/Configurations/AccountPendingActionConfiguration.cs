using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;

namespace User.Infrastructure.Data.Configurations
{
    public class AccountPendingActionConfiguration : IEntityTypeConfiguration<AccountPendingAction>
    {
        public void Configure(EntityTypeBuilder<AccountPendingAction> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedNever();

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.ToTable("AccountPendingActions");
        }
    }
}
