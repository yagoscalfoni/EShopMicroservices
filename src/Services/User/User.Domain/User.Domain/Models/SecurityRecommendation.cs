namespace User.Domain.Models
{
    public class SecurityRecommendation : Entity<Guid>
    {
        public UserId UserId { get; private set; } = default!;
        public string Description { get; private set; } = default!;

        private SecurityRecommendation()
        {
        }

        public static SecurityRecommendation Create(UserId userId, string description)
        {
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentException.ThrowIfNullOrWhiteSpace(description);

            return new SecurityRecommendation
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
