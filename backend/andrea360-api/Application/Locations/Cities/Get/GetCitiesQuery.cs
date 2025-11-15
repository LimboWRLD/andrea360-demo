using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Cities.Get;

public sealed class GetCitiesQuery : IQuery<List<City>>;