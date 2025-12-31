namespace User.Domain.Models
{
    public class User : Entity<UserId>
    {
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PhoneNumber { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public string PasswordSalt { get; private set; } = default!;
        public bool IsEmailVerified { get; private set; }
        public bool IsPhoneNumberVerified { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastLogin { get; private set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTime? LockoutEnd { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public string Gender { get; private set; } = default!;
        public string ProfilePictureUrl { get; private set; } = default!;
        public Guid? BillingAddressId { get; private set; }
        public Guid? ShippingAddressId { get; private set; }
        public string PreferredLanguage { get; private set; } = default!;
        public bool MarketingOptIn { get; private set; }
        public bool TwoFactorEnabled { get; private set; }
        public string Document { get; private set; } = string.Empty;
        public string GoogleId { get; private set; } = default!;
        public string AppleId { get; private set; } = default!;
        public string FacebookId { get; private set; } = default!;

        private User() { }

        public static User Create(
            UserId id,
            string firstName,
            string lastName,
            string email,
            string passwordHash,
            string passwordSalt,
            DateTime createdAt,
            string phoneNumber = "",
            bool isEmailVerified = false,
            bool isPhoneNumberVerified = false,
            DateTime? dateOfBirth = null,
            string gender = "",
            string profilePictureUrl = "",
            Guid? billingAddressId = null,
            Guid? shippingAddressId = null,
            string preferredLanguage = "en",
            bool marketingOptIn = false,
            bool twoFactorEnabled = false,
            string document = "",
            string googleId = "",
            string appleId = "",
            string facebookId = ""
        )
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
            ArgumentException.ThrowIfNullOrWhiteSpace(passwordSalt);

            return new User
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = createdAt,
                PhoneNumber = phoneNumber,
                IsEmailVerified = isEmailVerified,
                IsPhoneNumberVerified = isPhoneNumberVerified,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                ProfilePictureUrl = profilePictureUrl,
                BillingAddressId = billingAddressId,
                ShippingAddressId = shippingAddressId,
                PreferredLanguage = preferredLanguage,
                MarketingOptIn = marketingOptIn,
                TwoFactorEnabled = twoFactorEnabled,
                Document = document,
                GoogleId = googleId,
                AppleId = appleId,
                FacebookId = facebookId
            };
        }

        public void UpdateLastLogin(DateTime loginTime)
        {
            LastLogin = loginTime;
            FailedLoginAttempts = 0; // Reset failed login attempts after a successful login
        }

        public void IncrementFailedLoginAttempts()
        {
            FailedLoginAttempts++;
            if (FailedLoginAttempts >= 5) // Example threshold for locking out
            {
                LockoutEnd = DateTime.UtcNow.AddMinutes(15); // Lock out for 15 minutes
            }
        }

        public void ResetFailedLoginAttempts()
        {
            FailedLoginAttempts = 0;
            LockoutEnd = null;
        }

        public void UpdateProfile(string firstName, string lastName, string email, string phoneNumber)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
