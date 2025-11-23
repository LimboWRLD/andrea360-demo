using Application.Abstractions.Messaging;
using Application.Locations.Countries.Get;

namespace Application.Locations.Countries.Update;

public sealed record UpdateCountryCommand(Guid CountryId, string Name) : ICommand<GetCountryResponse>;

