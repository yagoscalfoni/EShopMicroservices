using System.ComponentModel.DataAnnotations;

namespace Shopping.Web.Models.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um email válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
