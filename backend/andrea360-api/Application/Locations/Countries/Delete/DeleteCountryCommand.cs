using Application.Abstractions.Messaging;

namespace Application.Locations.Countries.Delete;

public sealed record DeleteCountryCommand(Guid CountryId) : ICommand;
