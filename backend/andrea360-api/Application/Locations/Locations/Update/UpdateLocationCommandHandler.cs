using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Locations.Locations.Get;
using Domain.Locations;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Locations.Update
{
    internal sealed class UpdateLocationCommandHandler(IApplicationDbContext context, IMapper mapper) : ICommandHandler<UpdateLocationCommand, GetLocationResponse>
    {
        public async Task<Result<GetLocationResponse>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            Location? existingLocation = await context.Locations.FindAsync(request.LocationId, cancellationToken);

            if (existingLocation is null) return Result.Failure<GetLocationResponse>
                    (new Error("Location.NotFound", $"The Location with the id '{request.LocationId} was not found.'", ErrorType.NotFound));

            bool sameName = await context.Locations.AnyAsync(c => c.Name == request.Name, cancellationToken);

            if (sameName) return Result.Failure<GetLocationResponse>
                    (new Error("Location.NameTaken", $"The Location name: '{request.Name}' is taken.", ErrorType.Conflict));

            bool existingAddress = await context.Addresses
                .AnyAsync(a => a.Id == request.AddressId, cancellationToken);

            if (!existingAddress) return Result.Failure<GetLocationResponse>
                    (new Error("Address.NotFound", $"The Address with the id '{request.AddressId} was not found.'", ErrorType.NotFound));

            existingLocation.Name = request.Name;
            existingLocation.AddressId = request.AddressId;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<GetLocationResponse>(existingLocation));
        }
    }
}
