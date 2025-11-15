using Application.Locations.Countries.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Locations.Cities.Get
{
    public sealed class GetCityResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GetCountryResponse Country { get; set; }
    }
}
