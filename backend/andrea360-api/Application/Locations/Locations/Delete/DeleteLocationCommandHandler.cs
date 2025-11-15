using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Locations.Delete
{
    internal sealed class DeleteLocationCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteLocationCommand>
    {
        public async Task<Result> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            Location? existingLocation = await context.Locations.FindAsync(request.LocationId, cancellationToken);

            if (existingLocation is null) return Result.Failure<Guid>
                    (new Error("Location.NotFound", $"The Location with the id '{request.LocationId} was not found.'", ErrorType.NotFound));

            if (existingLocation.IsDeleted) Result.Failure<Guid>
                    (new Error("Location.AlreadyDeleted", $"The Location with the id: '{request.LocationId}' is already deleted.", ErrorType.Conflict));

            existingLocation.IsDeleted = true;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
