namespace Shopping.Web.Pages
{
    public class UserModel(IUserService userService, ILogger<UserModel> logger) 
        : PageModel
    {
        public ShoppingUserModel User { get; set; } = new ShoppingUserModel();

        public async Task<IActionResult> OnGetAsync()
        {
            User = new ShoppingUserModel
            {
                UserId = Guid.NewGuid(),
                Name = "John Doe",
                Email = "johndoe@example.com",
                PhoneNumber = "123-456-7890",
                Address = "123 Main St, Springfield, USA",
                DateOfBirth = new DateTime(1990, 5, 15),
                Gender = "Male",
                ProfileImageUrl = "https://example.com/images/profile.jpg",
                DateJoined = DateTime.Now.AddYears(-2),
                IsEmailVerified = true
            };

            return Page();
        }
        public IActionResult OnPostLogout()
        {
            // Remove o token de autenticação e outros dados de sessão
            HttpContext.Session.Remove("IsAuthenticated");
            HttpContext.Session.Remove("AuthToken");
            HttpContext.Session.Remove("UserName");

            logger.LogInformation("Usuário deslogado com sucesso.");

            // Redireciona para a página de login
            return RedirectToPage("/Login");
        }
    }
}
