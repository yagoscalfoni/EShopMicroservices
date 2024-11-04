using Microsoft.EntityFrameworkCore;

namespace User.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<User.Domain.Models.User> Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
