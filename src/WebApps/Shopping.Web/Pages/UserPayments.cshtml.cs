using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping.Web.Pages
{
    public class UserPaymentsModel : PageModel
    {
        public UserViewModel User { get; set; }

        public void OnGet()
        {
            // Simulando dados de exemplo para métodos de pagamento do usuário
            User = new UserViewModel
            {
                PaymentMethods = new List<PaymentMethodViewModel>
                {
                    new PaymentMethodViewModel
                    {
                        Id = Guid.NewGuid(),
                        CardType = "Visa",
                        LastFourDigits = "1234",
                        ExpirationDate = new DateTime(2025, 12, 1),
                        BillingAddress = "123 Main St, City, Country"
                    },
                    new PaymentMethodViewModel
                    {
                        Id = Guid.NewGuid(),
                        CardType = "Mastercard",
                        LastFourDigits = "5678",
                        ExpirationDate = new DateTime(2024, 7, 1),
                        BillingAddress = "456 Elm St, City, Country"
                    }
                }
            };
        }
    }

    public class UserViewModel
    {
        public List<PaymentMethodViewModel> PaymentMethods { get; set; }
    }

    public class PaymentMethodViewModel
    {
        public Guid Id { get; set; }
        public string CardType { get; set; }
        public string LastFourDigits { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string BillingAddress { get; set; }
    }
}
