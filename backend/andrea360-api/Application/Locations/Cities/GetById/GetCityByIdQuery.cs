using Application.Abstractions.Messaging;
using Application.Locations.Cities.Get;

namespace Application.Locations.Cities.GetById;

public sealed record GetCityByIdQuery(Guid CityId) : IQuery<GetCityResponse>;
