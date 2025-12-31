using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace User.Infrastructure.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.MigrateAsync().GetAwaiter().GetResult();

            await SeedAsync(context);
        }

        private static async Task SeedAsync(ApplicationDbContext context)
        {
            await SeedUsersAsync(context);
            await SeedAccountJourneyAsync(context);
        }

        private static async Task SeedUsersAsync(ApplicationDbContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                await context.Users.AddRangeAsync(InitialData.Users);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedAccountJourneyAsync(ApplicationDbContext context)
        {
            var overviews = InitialData.AccountOverviews.ToList();

            if (!await context.AccountOverviews.AnyAsync())
            {
                await context.AccountOverviews.AddRangeAsync(overviews);
            }

            if (!await context.AccountBenefits.AnyAsync())
            {
                await context.AccountBenefits.AddRangeAsync(overviews.SelectMany(o => o.Benefits));
            }

            if (!await context.AccountPendingActions.AnyAsync())
            {
                await context.AccountPendingActions.AddRangeAsync(overviews.SelectMany(o => o.PendingActions));
            }

            if (!await context.UserAddresses.AnyAsync())
            {
                await context.UserAddresses.AddRangeAsync(InitialData.UserAddresses);
            }

            if (!await context.PaymentMethods.AnyAsync())
            {
                await context.PaymentMethods.AddRangeAsync(InitialData.PaymentMethods);
            }

            if (!await context.SupportTickets.AnyAsync())
            {
                await context.SupportTickets.AddRangeAsync(InitialData.SupportTickets);
            }

            if (!await context.SecurityRecommendations.AnyAsync())
            {
                await context.SecurityRecommendations.AddRangeAsync(InitialData.SecurityRecommendations);
            }

            await context.SaveChangesAsync();
        }
    }
}
