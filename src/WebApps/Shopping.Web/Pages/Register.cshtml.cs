using System.Net;
using System.Text.Json;

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
        catch (ApiException apiEx)
        {
            _logger.LogWarning(apiEx, "API error during registration");
            if (apiEx.HasContent)
            {
                try
                {
                    var problem = await apiEx.Content.ReadFromJsonAsync<ProblemDetails>();
                    if (problem?.Extensions != null &&
                        problem.Extensions.TryGetValue("ValidationErrors", out var errorsObj) &&
                        errorsObj is JsonElement element &&
                        element.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var error in element.EnumerateArray())
                        {
                            if (error.TryGetProperty("ErrorMessage", out var msg))
                            {
                                ModelState.AddModelError(string.Empty, msg.GetString()!);
                            }
                        }
                    }
                    else if (problem != null)
                    {
                        ModelState.AddModelError(string.Empty, problem.Detail ?? "Registration failed");
                    }
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Registration failed");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Registration failed");
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user");
            ModelState.AddModelError(string.Empty, "Registration failed");
            return Page();
        }
    }
}
