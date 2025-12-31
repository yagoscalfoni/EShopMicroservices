using Microsoft.EntityFrameworkCore;
using User.Domain.Models;

namespace User.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<User.Domain.Models.User> Users { get; }
        DbSet<AccountOverview> AccountOverviews { get; }
        DbSet<AccountBenefit> AccountBenefits { get; }
        DbSet<AccountPendingAction> AccountPendingActions { get; }
        DbSet<UserAddress> UserAddresses { get; }
        DbSet<PaymentMethod> PaymentMethods { get; }
        DbSet<SupportTicket> SupportTickets { get; }
        DbSet<SecurityRecommendation> SecurityRecommendations { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
