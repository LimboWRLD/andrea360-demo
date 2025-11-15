using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Locations.Update;

public sealed record UpdateLocationCommand(Guid LocationId, string Name, Guid AddressId) : ICommand<Location>;

