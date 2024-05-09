﻿using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Domain.Models;
using System.Reflection;

namespace Ordering.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        { 

        }

        public DbSet<Customer> Customers => Set<Customer>(); 
        public DbSet<Product> Products => Set<Product>(); 
        public DbSet<Order> Orders => Set<Order>(); 
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public DbSet<Product> Product => throw new NotImplementedException();

        public Task<int> SaveChangesAsybc(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
