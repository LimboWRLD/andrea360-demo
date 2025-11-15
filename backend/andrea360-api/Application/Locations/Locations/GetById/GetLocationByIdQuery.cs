using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Locations.GetById;

public sealed record GetLocationByIdQuery(Guid LocationId) : IQuery<Location>;
