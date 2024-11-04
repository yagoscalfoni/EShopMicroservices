namespace User.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<User.Domain.Models.User> Users =>
            new List<User.Domain.Models.User>
            {
                User.Domain.Models.User.Create(
                    UserId.Of(new Guid("d1f8a1a7-b7b5-4d47-a799-df891f8bb123")),
                    firstName: "Alice",
                    lastName: "Smith",
                    email: "alice.smith@example.com",
                    passwordHash: "hashedpassword1", // Mocked password hash
                    passwordSalt: "salt1",
                    createdAt: DateTime.UtcNow,
                    phoneNumber: "+1234567890",
                    isEmailVerified: true,
                    isPhoneNumberVerified: true,
                    dateOfBirth: new DateTime(1990, 1, 1),
                    gender: "Female",
                    profilePictureUrl: "https://example.com/images/alice.jpg",
                    preferredLanguage: "en",
                    marketingOptIn: true,
                    twoFactorEnabled: false
                ),
                User.Domain.Models.User.Create(
                    UserId.Of(new Guid("e2b7f5cd-6d91-4d2b-9731-6d3bf58c64e7")),
                    firstName: "Bob",
                    lastName: "Johnson",
                    email: "bob.johnson@example.com",
                    passwordHash: "hashedpassword2", // Mocked password hash
                    passwordSalt: "salt2",
                    createdAt: DateTime.UtcNow,
                    phoneNumber: "+0987654321",
                    isEmailVerified: true,
                    isPhoneNumberVerified: false,
                    dateOfBirth: new DateTime(1985, 5, 20),
                    gender: "Male",
                    profilePictureUrl: "https://example.com/images/bob.jpg",
                    preferredLanguage: "en",
                    marketingOptIn: false,
                    twoFactorEnabled: true,
                    googleId: "google-id-12345"
                ),
                User.Domain.Models.User.Create(
                    UserId.Of(new Guid("f3d4c6e2-27b7-4ae2-88e1-3456f8a9b678")),
                    firstName: "Carol",
                    lastName: "Williams",
                    email: "carol.williams@example.com",
                    passwordHash: "hashedpassword3", // Mocked password hash
                    passwordSalt: "salt3",
                    createdAt: DateTime.UtcNow,
                    phoneNumber: "+1122334455",
                    isEmailVerified: false,
                    isPhoneNumberVerified: true,
                    dateOfBirth: new DateTime(1995, 8, 15),
                    gender: "Female",
                    profilePictureUrl: "https://example.com/images/carol.jpg",
                    preferredLanguage: "es",
                    marketingOptIn: true,
                    twoFactorEnabled: true,
                    appleId: "apple-id-54321"
                )
            };
    }
}
