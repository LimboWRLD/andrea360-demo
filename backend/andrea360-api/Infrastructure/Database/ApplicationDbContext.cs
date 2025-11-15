using Application.Abstractions.Interfaces;
using Domain.Billing;
using Domain.Catalog;
using Domain.Locations;
using Domain.Scheduling;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Database
{
    public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options), IApplicationDbContext
    {
        public DbSet<Country> Countries { get; }

        public DbSet<City> Cities { get; }

        public DbSet<Address> Addresses { get; }

        public DbSet<Location> Locations { get; }

        public DbSet<User> Users { get; }

        public DbSet<Transaction> Transactions { get; }

        public DbSet<UserService> UserServices { get; }

        public DbSet<Service> Services { get; }

        public DbSet<Reservation> Reservations { get; }

        public DbSet<Session> Sessions { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.HasDefaultSchema(Schemas.Default);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await Database.BeginTransactionAsync(cancellationToken);
        }

    }
}
