namespace User.Application.Dtos
{
    public record UserLoggedIntegrationEventDto
    {
        public Guid UserId { get; init; }
        public DateTime LoginTime { get; init; }
        public string Email { get; init; } = default!;
    }
}
