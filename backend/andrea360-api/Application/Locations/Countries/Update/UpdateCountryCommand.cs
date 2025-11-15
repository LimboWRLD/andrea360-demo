using Application.Abstractions.Messaging;

namespace Application.Locations.Countries.Update;

public sealed record UpdateCountryCommand(Guid CountryId, string Name) : ICommand<Guid>;

