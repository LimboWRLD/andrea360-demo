using Application.Abstractions.Messaging;
using Application.Locations.Cities.Get;

namespace Application.Locations.Cities.Update;

public sealed record UpdateCityCommand(Guid CityId, Guid CountryId, string Name) : ICommand<GetCityResponse>;
