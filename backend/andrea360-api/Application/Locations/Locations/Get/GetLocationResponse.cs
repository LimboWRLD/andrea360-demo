using Application.Locations.Addresses.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Locations.Locations.Get
{
    public class GetLocationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public GetAddressResponse Address { get; set; }
    }
}
