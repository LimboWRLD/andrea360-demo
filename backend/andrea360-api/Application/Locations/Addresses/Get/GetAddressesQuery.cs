using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Addresses.Get
{
    public class GetAddressesQuery : IQuery<List<GetAddressResponse>>
    {
    }
}
