using Domain.Common;

namespace Domain.Locations
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }

        public Guid AddressId { get; set; }

        public Address Address { get; set; }
    }
}
