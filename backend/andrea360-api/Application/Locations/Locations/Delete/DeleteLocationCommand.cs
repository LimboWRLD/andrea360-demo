using Application.Abstractions.Messaging;

namespace Application.Locations.Locations.Delete;

public sealed record DeleteLocationCommand(Guid LocationId) : ICommand;
