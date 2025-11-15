using Application.Abstractions.Messaging;

namespace Application.Locations.Cities.Update;

public sealed record UpdateCityCommand(Guid CityId, Guid CountryId, string Name) : ICommand<Guid>;
