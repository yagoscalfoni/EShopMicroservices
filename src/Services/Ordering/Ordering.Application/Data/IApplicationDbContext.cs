using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;

namespace Ordering.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; }
        DbSet<Product> Product { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
