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
                    // Armazena o estado de autenticação e token na sessão
                    HttpContext.Session.SetString("IsAuthenticated", "true");
                    HttpContext.Session.SetString("AuthToken", response.Token);
                    HttpContext.Session.SetString("UserName", response.Name);
                    HttpContext.Session.SetString("UserId", response.UserId.ToString());

                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Credenciais inválidas.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar autenticar o usuário.");
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao tentar autenticar. Tente novamente.");
            }

            return Page();
        }

    }
}
