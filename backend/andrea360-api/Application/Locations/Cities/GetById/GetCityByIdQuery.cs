using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Cities.GetById;

public sealed record GetCityByIdQuery(Guid CityId) : IQuery<City>;
