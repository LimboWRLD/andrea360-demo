using Domain.Catalog;
using Domain.Common;
using Domain.Users;

namespace Domain.Billing
{
    public class UserService : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ServiceId { get; set; }

        public Service Service { get; set; }

        public int RemainingSessions { get; set; } = 1;
    }
}
