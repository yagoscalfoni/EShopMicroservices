namespace User.Domain.Models
{
    public class AccountPendingAction : Entity<Guid>
    {
        public Guid OverviewId { get; private set; }
        public string Description { get; private set; } = default!;

        private AccountPendingAction()
        {
        }

        public static AccountPendingAction Create(Guid overviewId, string description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(description);

            return new AccountPendingAction
            {
                Id = Guid.NewGuid(),
                OverviewId = overviewId,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
