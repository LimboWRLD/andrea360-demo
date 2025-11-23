using Application.Abstractions.Messaging;
using Application.Locations.Locations.Get;

namespace Application.Locations.Locations.Create;
public sealed class CreateLocationCommand : ICommand<GetLocationResponse>
{
    public string Name { get; set; }

    public Guid AddressId { get; set; }
}

