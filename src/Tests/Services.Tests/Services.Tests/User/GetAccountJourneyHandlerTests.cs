using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using User.Application.Users.Queries.GetAccountJourney;
using User.Domain.Models;
using User.Infrastructure.Data;

namespace Services.Tests.User;

public class GetAccountJourneyHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCompleteJourneySnapshot()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        var userId = UserId.Of(Guid.NewGuid());
        var user = User.Domain.Models.User.Create(
            userId,
            firstName: "Mariana",
            lastName: "Silva",
            email: "mariana.teste@email.com",
            passwordHash: "hash",
            passwordSalt: "salt",
            createdAt: DateTime.UtcNow,
            phoneNumber: "+55 (11) 98888-1111",
            marketingOptIn: true,
            document: "123.456.789-10");

        var overview = AccountOverview.Create(
            userId,
            nextDeliveryWindow: "Entrega prevista entre 10h - 14h",
            loyaltyLevel: "Cliente Gold",
            lastOrderId: "#548712",
            lastOrderTotal: 389.90m,
            benefits: new[] { "Frete grátis" },
            pendingActions: new[] { "Confirmar endereço" });

        context.Users.Add(user);
        context.AccountOverviews.Add(overview);
        context.AccountBenefits.AddRange(overview.Benefits);
        context.AccountPendingActions.AddRange(overview.PendingActions);
        context.UserAddresses.Add(UserAddress.Create(userId, "Casa", "Mariana", "Rua 1", "São Paulo", "SP", "00000-000", true));
        context.PaymentMethods.Add(PaymentMethod.Create(userId, "Visa", "4829", "08/27", true, "Credit Card"));
        context.SupportTickets.Add(SupportTicket.Create(userId, "SUP-1000", "Dúvida", "Aberto", DateTime.UtcNow));
        context.SecurityRecommendations.Add(SecurityRecommendation.Create(userId, "Ative 2FA"));

        await context.SaveChangesAsync();

        var handler = new GetAccountJourneyHandler(context);

        var result = await handler.Handle(new GetAccountJourneyQuery(userId.Value), CancellationToken.None);

        result.Journey.Profile.Name.Should().Contain("Mariana");
        result.Journey.Profile.Document.Should().Be("123.456.789-10");
        result.Journey.Addresses.Should().ContainSingle(a => a.Default);
        result.Journey.PaymentMethods.Should().ContainSingle(p => p.Preferred);
        result.Journey.SupportTickets.Should().Contain(ticket => ticket.Id == "SUP-1000");
    }
}
