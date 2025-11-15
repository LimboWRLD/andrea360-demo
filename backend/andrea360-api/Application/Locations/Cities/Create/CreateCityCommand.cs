using Application.Abstractions.Messaging;
using Application.Locations.Cities.Get;

namespace Application.Locations.Cities.Create;

public sealed class CreateCityCommand : ICommand<GetCityResponse>
{
    public string Name { get; set; }

    public Guid CountryId { get; set; }
}
