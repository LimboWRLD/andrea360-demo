using Application.Abstractions.Messaging;

namespace Application.Locations.Addresses.Update;

public sealed record UpdateAddressCommand(Guid AddressId, Guid CityId, string Street, string Number) : ICommand<Guid>;
