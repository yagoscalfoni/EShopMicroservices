using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping.Web.Pages
{
    public class UserDetailsModel : PageModel
    {
        [BindProperty]
        public UserModelDetails User { get; set; } = new UserModelDetails();

        public void OnGet()
        {
            // Carregar dados do usu�rio para exibir na p�gina
            User = new UserModelDetails
            {
                FirstName = "Jo�o",
                LastName = "Silva",
                Email = "joao.silva@email.com",
                PhoneNumber = "(27) 98765-4321",
                DateOfBirth = new DateTime(1990, 5, 20),
                StreetAddress = "Rua das Flores, 123",
                City = "S�o Paulo",
                PostalCode = "12345-678",
                Country = "Brasil",
                ReceivePromotions = true
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Simular salvamento de dados do usu�rio
            return RedirectToPage("UserProfile");
        }
    }

    public class UserModelDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool ReceivePromotions { get; set; }
    }
}
