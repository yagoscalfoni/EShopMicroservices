namespace User.Domain.Models
{
    public class SupportTicket : Entity<Guid>
    {
        public UserId UserId { get; private set; } = default!;
        public string TicketNumber { get; private set; } = default!;
        public string Subject { get; private set; } = default!;
        public string Status { get; private set; } = default!;
        public DateTime UpdatedAt { get; private set; }

        private SupportTicket()
        {
        }

        public static SupportTicket Create(UserId userId, string ticketNumber, string subject, string status, DateTime updatedAt)
        {
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentException.ThrowIfNullOrWhiteSpace(ticketNumber);
            ArgumentException.ThrowIfNullOrWhiteSpace(subject);
            ArgumentException.ThrowIfNullOrWhiteSpace(status);

            return new SupportTicket
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TicketNumber = ticketNumber,
                Subject = subject,
                Status = status,
                UpdatedAt = updatedAt,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
