using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Login;
using Shopping.Web.Services;

namespace Shopping.Web.Pages;

public class ForgotPasswordModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<ForgotPasswordModel> _logger;

    public ForgotPasswordModel(IUserService userService, ILogger<ForgotPasswordModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    public RecoverPasswordModel RecoverData { get; set; } = new RecoverPasswordModel();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await _userService.RecoverPassword(new RecoverPasswordRequest(RecoverData.Email));
            TempData["Message"] = "Se o email existir, instruções serão enviadas.";
            return RedirectToPage("/Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao solicitar recuperação de senha");
            ModelState.AddModelError(string.Empty, "Erro ao solicitar recuperação de senha");
            return Page();
        }
    }
}
