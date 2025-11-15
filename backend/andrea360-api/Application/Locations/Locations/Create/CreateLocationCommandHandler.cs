using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Domain.Locations;

namespace Application.Locations.Locations.Create
{
    internal sealed class CreateLocationCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateLocationCommand, Location>
    {
        public async Task<Result<Location>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            bool exinsting = await context.Locations.AnyAsync(c => c.Name == request.Name);
            if (exinsting) return Result.Failure<Location>
                           (new Error("Location.NameTaken", $"The location name: '{request.Name}' is taken", ErrorType.Conflict));

            bool existingAddress = await context.Addresses.AnyAsync(a => a.Id == request.AddressId, cancellationToken);
            if (!existingAddress) return Result.Failure<Location>
                           (new Error("Location.AddressNotFound", $"The address with id: '{request.AddressId}' was not found", ErrorType.NotFound));

            var location = new Location
            {
                Name = request.Name,
                AddressId = request.AddressId,
                IsDeleted = false
            };

            context.Locations.Add(location);

            await context.SaveChangesAsync(cancellationToken);

            return location;
        }
    }
}
