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
    }
}
