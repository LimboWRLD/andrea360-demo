using Domain.Common;
using Domain.Users;

namespace Domain.Scheduling
{
    public class Reservation : BaseEntity
    {
        public Guid SessionId { get; set; }

        public Session Session { get; set; }

        public DateTime ReservedAt { get; set; }

        public Guid UserId { get; set; }    

        public User User { get; set; }

        public Boolean IsCancelled { get; set; }
    }
}
