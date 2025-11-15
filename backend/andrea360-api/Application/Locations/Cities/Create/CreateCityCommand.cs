using Application.Abstractions.Messaging;

namespace Application.Locations.Cities.Create;

public sealed class CreateCityCommand : ICommand<Guid>
{
    public string Name { get; set; }

    public Guid CountryId { get; set; }
}
