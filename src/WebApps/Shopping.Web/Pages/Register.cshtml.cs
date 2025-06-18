namespace Shopping.Web.Pages;

public class RegisterModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<RegisterModel> _logger;

    public RegisterModel(IUserService userService, ILogger<RegisterModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    public RegisterUserModel RegisterData { get; set; } = new RegisterUserModel();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            var request = new RegisterUserRequest(RegisterData.FirstName, RegisterData.LastName, RegisterData.Email, RegisterData.Password);
            await _userService.RegisterUser(request);
            return RedirectToPage("/Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user");
            ModelState.AddModelError(string.Empty, "Registration failed");
            return Page();
        }
    }
}
