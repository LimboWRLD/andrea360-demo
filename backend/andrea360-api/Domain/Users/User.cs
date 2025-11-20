using Domain.Billing;
using Domain.Common;
using Domain.Locations;
using Domain.Scheduling;

namespace Domain.Users
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Guid LocationId { get; set; }

        public Location Location { get; set; }

        public string? StripeCustomerId { get; set; }

        public string KeycloakId { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public ICollection<UserService> UserSessions { get; set; } = new List<UserService>();

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public ICollection<Reservation> Reservations { get; set; }  = new List<Reservation>();
    }
}
