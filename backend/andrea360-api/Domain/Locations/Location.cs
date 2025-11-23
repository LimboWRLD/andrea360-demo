using Domain.Common;
using Domain.Scheduling;

namespace Domain.Locations
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }

        public Guid AddressId { get; set; }

        public Address Address { get; set; }

        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
