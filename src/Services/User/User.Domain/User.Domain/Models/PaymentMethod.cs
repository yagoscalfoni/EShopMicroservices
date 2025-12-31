namespace User.Domain.Models
{
    public class PaymentMethod : Entity<Guid>
    {
        public UserId UserId { get; private set; } = default!;
        public string Brand { get; private set; } = default!;
        public string Last4 { get; private set; } = default!;
        public string Expiry { get; private set; } = default!;
        public bool Preferred { get; private set; }
        public string Type { get; private set; } = default!;

        private PaymentMethod()
        {
        }

        public static PaymentMethod Create(UserId userId, string brand, string last4, string expiry, bool preferred, string type)
        {
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentException.ThrowIfNullOrWhiteSpace(brand);
            ArgumentException.ThrowIfNullOrWhiteSpace(last4);
            ArgumentException.ThrowIfNullOrWhiteSpace(expiry);
            ArgumentException.ThrowIfNullOrWhiteSpace(type);

            return new PaymentMethod
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Brand = brand,
                Last4 = last4,
                Expiry = expiry,
                Preferred = preferred,
                Type = type,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
