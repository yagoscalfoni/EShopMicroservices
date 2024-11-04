using User.Application.Dtos;
using User.Domain.Models;

namespace User.Application.Extensions
{
    public static class UserExtensions
    {
        public static UserLoggedIntegrationEventDto ToUserLoggedDto(this User.Domain.Models.User user)
        {
            return LoginFromUserDto(user);
        }

        private static UserLoggedIntegrationEventDto LoginFromUserDto(User.Domain.Models.User user)
        {
            return new UserLoggedIntegrationEventDto
            {
                Email = user.Email,
                LoginTime = user.LastLogin.GetValueOrDefault(),
                UserId = user.Id.Value
            };
        }
        public static UserDto ToUserDto(this User.Domain.Models.User user)
        {
            return new UserDto
            {
                UserId = user.Id.Value,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailVerified = user.IsEmailVerified,
                IsPhoneNumberVerified = user.IsPhoneNumberVerified,
                CreatedAt = user.CreatedAt,
                LastLogin = user.LastLogin,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                ProfilePictureUrl = user.ProfilePictureUrl,
                PreferredLanguage = user.PreferredLanguage,
                MarketingOptIn = user.MarketingOptIn,
                TwoFactorEnabled = user.TwoFactorEnabled
            };
        }
    }
}
