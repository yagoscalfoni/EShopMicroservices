namespace Shopping.Web.Pages
{
    public class UserModel(IUserService userService, ILogger<UserModel> logger) 
        : PageModel
    {
        public ShoppingUserModel User { get; set; } = new ShoppingUserModel();

        public async Task<IActionResult> OnGetAsync()
        {
            User = await userService.LoadUser();

            return Page();
        }
    }
}
