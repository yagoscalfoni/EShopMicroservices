using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;

namespace User.Infrastructure.Data.Configurations
{
    public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
    {
        public void Configure(EntityTypeBuilder<SupportTicket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedNever();

            builder.Property(t => t.UserId)
                .HasConversion(id => id.Value, value => UserId.Of(value))
                .IsRequired();

            builder.Property(t => t.TicketNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.ToTable("SupportTickets");
        }
    }
}
