using Application.Abstractions.Messaging;

namespace Application.Locations.Cities.Delete;

public sealed record DeleteCityCommand(Guid CityId) : ICommand;

