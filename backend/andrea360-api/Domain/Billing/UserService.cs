using Domain.Catalog;
using Domain.Common;

namespace Domain.Billing
{
    public class UserService : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }

        public Service Service { get; set; }

        public int RemainingSessions { get; set; } = 1;
    }
}
