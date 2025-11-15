using Application.Locations.Cities.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Locations.Addresses.Get
{
    public sealed class GetAddressResponse
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public GetCityResponse City { get; set; }
    }
}
