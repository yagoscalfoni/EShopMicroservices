namespace User.Domain.Models
{
    public class AccountBenefit : Entity<Guid>
    {
        public Guid OverviewId { get; private set; }
        public string Description { get; private set; } = default!;

        private AccountBenefit()
        {
        }

        public static AccountBenefit Create(Guid overviewId, string description)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(description);

            return new AccountBenefit
            {
                Id = Guid.NewGuid(),
                OverviewId = overviewId,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
