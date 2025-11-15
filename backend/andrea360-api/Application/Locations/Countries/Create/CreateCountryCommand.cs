using Application.Abstractions.Messaging;

namespace Application.Locations.Countries.Create;
public sealed class CreateCountryCommand : ICommand<Guid>
{
    public string Name { get; set; }
}

