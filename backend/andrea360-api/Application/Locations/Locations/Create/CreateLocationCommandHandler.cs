using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Locations.Locations.Get;
using Domain.Locations;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Locations.Create
{
    internal sealed class CreateLocationCommandHandler(IApplicationDbContext context, IMapper mapper) : ICommandHandler<CreateLocationCommand, GetLocationResponse>
    {
        public async Task<Result<GetLocationResponse>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            bool exinsting = await context.Locations.AnyAsync(c => c.Name == request.Name);
            if (exinsting) return Result.Failure<GetLocationResponse>
                           (new Error("Location.NameTaken", $"The location name: '{request.Name}' is taken", ErrorType.Conflict));

            bool existingAddress = await context.Addresses.AnyAsync(a => a.Id == request.AddressId, cancellationToken);
            if (!existingAddress) return Result.Failure<GetLocationResponse>
                           (new Error("Location.AddressNotFound", $"The address with id: '{request.AddressId}' was not found", ErrorType.NotFound));

            var location = new Location
            {
                Name = request.Name,
                AddressId = request.AddressId,
                IsDeleted = false
            };

            context.Locations.Add(location);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<GetLocationResponse>(location));
        }
    }
}
