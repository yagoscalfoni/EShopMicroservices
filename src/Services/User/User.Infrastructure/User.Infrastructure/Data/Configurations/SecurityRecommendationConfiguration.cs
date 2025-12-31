using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;

namespace User.Infrastructure.Data.Configurations
{
    public class SecurityRecommendationConfiguration : IEntityTypeConfiguration<SecurityRecommendation>
    {
        public void Configure(EntityTypeBuilder<SecurityRecommendation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .ValueGeneratedNever();

            builder.Property(r => r.UserId)
                .HasConversion(id => id.Value, value => UserId.Of(value))
                .IsRequired();

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.ToTable("SecurityRecommendations");
        }
    }
}
