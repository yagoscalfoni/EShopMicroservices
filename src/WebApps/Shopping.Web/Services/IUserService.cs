using System.Net;

namespace Shopping.Web.Services
{
    public interface IUserService
    {
        [Get("/user-service/user/{userName}")]
        Task<GetUserResponse> GetUser(string userName);

        public async Task<ShoppingUserModel> LoadUser()
        {
            // Get Basket If Not Exist Create New Basket with Default Logged In User Name: swn
            var userName = "swn";
            var id = Guid.NewGuid();
            ShoppingUserModel user;

            try
            {
                var getUserResponse = await GetUser(userName);
                user = getUserResponse.User;
            }
            catch (ApiException apiException) when (apiException.StatusCode == HttpStatusCode.NotFound)
            {
                user = new ShoppingUserModel
                {
                     UserId = id,
                };
            }

            return user;
        }
    }
}
