namespace User.Domain.Models
{
    public class AccountOverview : Entity<Guid>
    {
        public UserId UserId { get; private set; } = default!;
        public string NextDeliveryWindow { get; private set; } = default!;
        public string LoyaltyLevel { get; private set; } = default!;
        public string LastOrderId { get; private set; } = default!;
        public decimal LastOrderTotal { get; private set; }
        public ICollection<AccountBenefit> Benefits { get; private set; } = new List<AccountBenefit>();
        public ICollection<AccountPendingAction> PendingActions { get; private set; } = new List<AccountPendingAction>();

        private AccountOverview()
        {
        }

        public static AccountOverview Create(
            UserId userId,
            string nextDeliveryWindow,
            string loyaltyLevel,
            string lastOrderId,
            decimal lastOrderTotal,
            IEnumerable<string> benefits,
            IEnumerable<string> pendingActions)
        {
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentException.ThrowIfNullOrWhiteSpace(nextDeliveryWindow);
            ArgumentException.ThrowIfNullOrWhiteSpace(loyaltyLevel);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastOrderId);

            var overviewId = Guid.NewGuid();
            var overview = new AccountOverview
            {
                Id = overviewId,
                UserId = userId,
                NextDeliveryWindow = nextDeliveryWindow,
                LoyaltyLevel = loyaltyLevel,
                LastOrderId = lastOrderId,
                LastOrderTotal = lastOrderTotal,
                CreatedAt = DateTime.UtcNow
            };

            overview.Benefits = benefits
                .Where(b => !string.IsNullOrWhiteSpace(b))
                .Select(b => AccountBenefit.Create(overviewId, b))
                .ToList();

            overview.PendingActions = pendingActions
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(a => AccountPendingAction.Create(overviewId, a))
                .ToList();

            return overview;
        }
    }
}
