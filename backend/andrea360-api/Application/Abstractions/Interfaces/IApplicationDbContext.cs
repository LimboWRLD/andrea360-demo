using Domain.Billing;
using Domain.Catalog;
using Domain.Locations;
using Domain.Scheduling;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Abstractions.Interfaces
{
    public interface IApplicationDbContext
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

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
