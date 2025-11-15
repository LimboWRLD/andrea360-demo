using Domain.Common;

namespace Domain.Locations
{
    public class Address : BaseEntity
    {
        public string Street { get; set; }

        public string Number { get; set; }

        public Guid CityId { get; set; }

        public City City { get; set; }
    }
}
