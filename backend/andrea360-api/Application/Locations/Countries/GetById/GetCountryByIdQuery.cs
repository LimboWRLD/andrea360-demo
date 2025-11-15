using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Countries.GetById;

public sealed record GetCountryByIdQuery(Guid CountryId) : IQuery<Country>;
