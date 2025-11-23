using Application.Abstractions.Messaging;
using Application.Locations.Locations.Get;
using Domain.Locations;

namespace Application.Locations.Locations.GetById;

public sealed record GetLocationByIdQuery(Guid LocationId) : IQuery<GetLocationResponse>;
