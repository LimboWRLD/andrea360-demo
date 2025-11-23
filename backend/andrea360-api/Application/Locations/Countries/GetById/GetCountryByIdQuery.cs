using Application.Abstractions.Messaging;
using Application.Locations.Countries.Get;
using Domain.Locations;

namespace Application.Locations.Countries.GetById;

public sealed record GetCountryByIdQuery(Guid CountryId) : IQuery<GetCountryResponse>;
