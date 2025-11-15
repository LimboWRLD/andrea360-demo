using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Locations.Get;

public sealed record GetLocationsQuery : IQuery<List<Location>>;

