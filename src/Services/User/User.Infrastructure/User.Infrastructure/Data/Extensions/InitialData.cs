using User.Domain.Models;

namespace User.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static readonly UserId JourneyUserId = UserId.Of(new Guid("d1f8a1a7-b7b5-4d47-a799-df891f8bb123"));

        public static IEnumerable<User.Domain.Models.User> Users =>
            new List<User.Domain.Models.User>
            {
                User.Domain.Models.User.Create(
                    JourneyUserId,
                    firstName: "Mariana",
                    lastName: "Silva",
                    email: "mariana.silva@email.com",
                    passwordHash: " hashedpassword1",
                    passwordSalt: "salt1",
                    createdAt: DateTime.UtcNow,
                    phoneNumber: "+55 (11) 98888-1111",
                    isEmailVerified: true,
                    isPhoneNumberVerified: true,
                    dateOfBirth: new DateTime(1992, 6, 12),
                    gender: "Female",
                    profilePictureUrl: "https://example.com/images/mariana.jpg",
                    preferredLanguage: "pt-BR",
                    marketingOptIn: true,
                    twoFactorEnabled: false,
                    document: "123.456.789-10"
                ),
                User.Domain.Models.User.Create(
                    UserId.Of(new Guid("e2b7f5cd-6d91-4d2b-9731-6d3bf58c64e7")),
                    firstName: "Bob",
                    lastName: "Johnson",
                    email: "bob.johnson@example.com",
                    passwordHash: "hashedpassword2",
                    passwordSalt: "salt2",
                    createdAt: DateTime.UtcNow,
                    phoneNumber: "+0987654321",
                    isEmailVerified: true,
                    isPhoneNumberVerified: false,
                    dateOfBirth: new DateTime(1985, 5, 20),
                    gender: "Male",
                    profilePictureUrl: "https://example.com/images/bob.jpg",
                    preferredLanguage: "en",
                    marketingOptIn: false,
                    twoFactorEnabled: true,
                    document: "987.654.321-00",
                    googleId: "google-id-12345"
                ),
                User.Domain.Models.User.Create(
                    UserId.Of(new Guid("f3d4c6e2-27b7-4ae2-88e1-3456f8a9b678")),
                    firstName: "Carol",
                    lastName: "Williams",
                    email: "carol.williams@example.com",
                    passwordHash: "hashedpassword3",
                    passwordSalt: "salt3",
                    createdAt: DateTime.UtcNow,
                    phoneNumber: "+1122334455",
                    isEmailVerified: false,
                    isPhoneNumberVerified: true,
                    dateOfBirth: new DateTime(1995, 8, 15),
                    gender: "Female",
                    profilePictureUrl: "https://example.com/images/carol.jpg",
                    preferredLanguage: "es",
                    marketingOptIn: true,
                    twoFactorEnabled: true,
                    document: "321.654.987-00",
                    appleId: "apple-id-54321"
                )
            };

        public static IEnumerable<AccountOverview> AccountOverviews =>
            new List<AccountOverview>
            {
                AccountOverview.Create(
                    JourneyUserId,
                    nextDeliveryWindow: "Entrega prevista entre 10h - 14h no dia 25/06",
                    loyaltyLevel: "Cliente Gold",
                    lastOrderId: "#548712",
                    lastOrderTotal: 389.90m,
                    benefits: new []
                    {
                        "Frete grátis acima de R$ 99",
                        "Troca rápida em até 30 dias",
                        "Atendimento prioritário"
                    },
                    pendingActions: new []
                    {
                        "Adicionar endereço de trabalho",
                        "Salvar forma de pagamento preferida",
                        "Ativar notificação de ofertas"
                    })
            };

        public static IEnumerable<UserAddress> UserAddresses =>
            new List<UserAddress>
            {
                UserAddress.Create(
                    JourneyUserId,
                    label: "Casa",
                    receiver: "Mariana Silva",
                    street: "Rua das Flores, 123 - Apto 45",
                    city: "São Paulo",
                    state: "SP",
                    zipCode: "01310-930",
                    isDefault: true,
                    deliveryNotes: "Interfone na portaria e suba até o 4º andar"),
                UserAddress.Create(
                    JourneyUserId,
                    label: "Trabalho",
                    receiver: "Mariana Silva",
                    street: "Av. Paulista, 500 - 12º andar",
                    city: "São Paulo",
                    state: "SP",
                    zipCode: "01310-100",
                    isDefault: false,
                    deliveryNotes: "Recepção aceita entregas até 18h")
            };

        public static IEnumerable<PaymentMethod> PaymentMethods =>
            new List<PaymentMethod>
            {
                PaymentMethod.Create(JourneyUserId, "Visa", "4829", "08/27", preferred: true, type: "Credit Card"),
                PaymentMethod.Create(JourneyUserId, "Mastercard", "1038", "03/26", preferred: false, type: "Debit Card"),
                PaymentMethod.Create(JourneyUserId, "Pix", "Conta Itaú", "Disponível", preferred: false, type: "Pix")
            };

        public static IEnumerable<SupportTicket> SupportTickets =>
            new List<SupportTicket>
            {
                SupportTicket.Create(JourneyUserId, "SUP-1023", "Status do pedido #548712", "Respondido", DateTime.UtcNow.AddHours(-2)),
                SupportTicket.Create(JourneyUserId, "SUP-0974", "Troca de produto - pedido #540120", "Concluído", new DateTime(2024, 6, 5)),
                SupportTicket.Create(JourneyUserId, "SUP-0955", "Alterar endereço principal", "Aberto", new DateTime(2024, 6, 3))
            };

        public static IEnumerable<SecurityRecommendation> SecurityRecommendations =>
            new List<SecurityRecommendation>
            {
                SecurityRecommendation.Create(JourneyUserId, "Ative a confirmação em dois fatores"),
                SecurityRecommendation.Create(JourneyUserId, "Atualize sua senha a cada 90 dias")
            };
    }
}
