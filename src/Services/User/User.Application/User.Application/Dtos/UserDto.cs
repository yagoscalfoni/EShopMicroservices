namespace User.Application.Dtos
{
    public record UserDto
    {
        public Guid UserId { get; init; }
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string PhoneNumber { get; init; } = default!;
        public bool IsEmailVerified { get; init; }
        public bool IsPhoneNumberVerified { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? LastLogin { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string Gender { get; init; } = default!;
        public string ProfilePictureUrl { get; init; } = default!;
        public string PreferredLanguage { get; init; } = default!;
        public bool MarketingOptIn { get; init; }
        public bool TwoFactorEnabled { get; init; }
    }
}
