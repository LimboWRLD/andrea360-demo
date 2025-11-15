using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Locations.Update
{
    internal sealed class UpdateLocationCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateLocationCommand, Location>
    {
        public async Task<Result<Location>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            Location? existingLocation = await context.Locations.FindAsync(request.LocationId, cancellationToken);

            if (existingLocation is null) return Result.Failure<Location>
                    (new Error("Location.NotFound", $"The Location with the id '{request.LocationId} was not found.'", ErrorType.NotFound));

            bool sameName = await context.Locations.AnyAsync(c => c.Name == request.Name, cancellationToken);

            if (sameName) return Result.Failure<Location>
                    (new Error("Location.NameTaken", $"The Location name: '{request.Name}' is taken.", ErrorType.Conflict));

            bool existingAddress = await context.Addresses
                .AnyAsync(a => a.Id == request.AddressId, cancellationToken);

            if (!existingAddress) return Result.Failure<Location>
                    (new Error("Address.NotFound", $"The Address with the id '{request.AddressId} was not found.'", ErrorType.NotFound));

            existingLocation.Name = request.Name;
            existingLocation.AddressId = request.AddressId;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(existingLocation);
        }
    }
}
