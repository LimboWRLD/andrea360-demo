using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Locations.Create;
public sealed class CreateLocationCommand : ICommand<Location>
{
    public string Name { get; set; }

    public Guid AddressId { get; set; }
}

