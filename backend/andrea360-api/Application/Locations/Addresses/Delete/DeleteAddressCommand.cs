using Application.Abstractions.Messaging;

namespace Application.Locations.Addresses.Delete;

public sealed record DeleteAddressCommand(Guid AddressId) : ICommand;
