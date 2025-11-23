using Application.Abstractions.Messaging;
using Application.Locations.Addresses.Get;

namespace Application.Locations.Addresses.Create;

public sealed class CreateAddressCommand : ICommand<GetAddressResponse>
{
    public string Number { get; set; }

    public string Street { get; set; }

    public Guid CityId { get; set; }
}
