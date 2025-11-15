using Application.Abstractions.Messaging;
using Application.Locations.Addresses.Get;

namespace Application.Locations.Addresses.Update;

public sealed record UpdateAddressCommand(Guid AddressId, Guid CityId, string Street, string Number) : ICommand<GetAddressResponse>;
