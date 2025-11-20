using Domain.Billing;
using Domain.Catalog;
using Domain.Common;
using Domain.Locations;

namespace Domain.Scheduling
{
    public class Session : BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid LocationId { get; set; }
        public Location Location { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }

        public int MaxCapacity { get; set; }

        public int CurrentCapacity { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<UserService> UserServices { get; set; } = new List<UserService>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
