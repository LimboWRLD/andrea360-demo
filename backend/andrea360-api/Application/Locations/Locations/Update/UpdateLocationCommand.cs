using Application.Abstractions.Messaging;
using Application.Locations.Locations.Get;
using Domain.Locations;

namespace Application.Locations.Locations.Update;

public sealed record UpdateLocationCommand(Guid LocationId, string Name, Guid AddressId) : ICommand<GetLocationResponse>;

