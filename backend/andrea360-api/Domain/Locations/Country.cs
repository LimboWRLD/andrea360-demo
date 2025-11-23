using Domain.Common;

namespace Domain.Locations
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
