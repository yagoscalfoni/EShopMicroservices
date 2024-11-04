namespace Shopping.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IUserService userService, ILogger<LoginModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [BindProperty]
        public Models.Login.LoginModel LoginData { get; set; } = new Models.Login.LoginModel();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                var request = new AuthenticateUserRequest(LoginData.Email, LoginData.Password);
                var response = await _userService.AuthenticateUser(request);

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    // Armazena o estado de autentica��o e token na sess�o
                    HttpContext.Session.SetString("IsAuthenticated", "true");
                    HttpContext.Session.SetString("AuthToken", response.Token);
                    HttpContext.Session.SetString("UserName", response.Name);

                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Credenciais inv�lidas.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar autenticar o usu�rio.");
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao tentar autenticar. Tente novamente.");
            }

            return Page();
        }

    }
}
