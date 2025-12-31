using System.Reflection;
using User.Application.Data;
using User.Domain.Models;

namespace User.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User.Domain.Models.User> Users => Set<User.Domain.Models.User>();
        public DbSet<AccountOverview> AccountOverviews => Set<AccountOverview>();
        public DbSet<AccountBenefit> AccountBenefits => Set<AccountBenefit>();
        public DbSet<AccountPendingAction> AccountPendingActions => Set<AccountPendingAction>();
        public DbSet<UserAddress> UserAddresses => Set<UserAddress>();
        public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
        public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
        public DbSet<SecurityRecommendation> SecurityRecommendations => Set<SecurityRecommendation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
