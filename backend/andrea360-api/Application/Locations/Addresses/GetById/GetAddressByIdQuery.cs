using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Addresses.GetById;

public sealed record GetAddressByIdQuery(Guid AddressId) : IQuery<Address>;
