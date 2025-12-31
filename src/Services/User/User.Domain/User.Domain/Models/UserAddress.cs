namespace User.Domain.Models
{
    public class UserAddress : Entity<Guid>
    {
        public UserId UserId { get; private set; } = default!;
        public string Label { get; private set; } = default!;
        public string Receiver { get; private set; } = default!;
        public string Street { get; private set; } = default!;
        public string City { get; private set; } = default!;
        public string State { get; private set; } = default!;
        public string ZipCode { get; private set; } = default!;
        public bool IsDefault { get; private set; }
        public string? DeliveryNotes { get; private set; }

        private UserAddress()
        {
        }

        public static UserAddress Create(
            UserId userId,
            string label,
            string receiver,
            string street,
            string city,
            string state,
            string zipCode,
            bool isDefault,
            string? deliveryNotes = null)
        {
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentException.ThrowIfNullOrWhiteSpace(label);
            ArgumentException.ThrowIfNullOrWhiteSpace(receiver);
            ArgumentException.ThrowIfNullOrWhiteSpace(street);
            ArgumentException.ThrowIfNullOrWhiteSpace(city);
            ArgumentException.ThrowIfNullOrWhiteSpace(state);
            ArgumentException.ThrowIfNullOrWhiteSpace(zipCode);

            return new UserAddress
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Label = label,
                Receiver = receiver,
                Street = street,
                City = city,
                State = state,
                ZipCode = zipCode,
                IsDefault = isDefault,
                DeliveryNotes = deliveryNotes,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
