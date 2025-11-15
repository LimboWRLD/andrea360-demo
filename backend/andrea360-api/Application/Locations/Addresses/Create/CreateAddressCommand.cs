using Application.Abstractions.Messaging;

namespace Application.Locations.Addresses.Create;

public sealed class CreateAddressCommand : ICommand<Guid>
{
    public string Number { get; set; }

    public string Street { get; set; }

    public Guid CityId { get; set; }
}
