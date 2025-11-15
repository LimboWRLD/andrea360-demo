using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Countries.Get;

public sealed record GetCountriesQuery : IQuery<List<GetCountryResponse>>;

