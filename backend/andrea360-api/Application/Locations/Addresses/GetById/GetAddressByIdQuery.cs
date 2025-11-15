using Application.Abstractions.Messaging;
using Application.Locations.Addresses.Get;
using Domain.Locations;

namespace Application.Locations.Addresses.GetById;

public sealed record GetAddressByIdQuery(Guid AddressId) : IQuery<GetAddressResponse>;
