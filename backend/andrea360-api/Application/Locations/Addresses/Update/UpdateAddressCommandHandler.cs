using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Addresses.Update
{
    internal sealed class UpdateOrderCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateAddressCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            Address? existingAddress = await context.Addresses.FindAsync(request.AddressId, cancellationToken);

            if (existingAddress is null) return Result.Failure<Guid>
                    (new Error("Address.NotFound", $"The address with the id: '{request.AddressId}' was not found.", ErrorType.NotFound));


            bool sameStreetAndNumber = await context.Addresses.AnyAsync(c => c.Number == request.Number && c.Street == request.Street && c.CityId == request.CityId, cancellationToken);
            if (sameStreetAndNumber) Result.Failure<Guid>
                    (new Error("Address.Taken", $"The address : '{request.Street}' number : '{request.Number}' is taken.", ErrorType.Conflict));

            existingAddress.Street = request.Street;
            existingAddress.Number = request.Number;
            existingAddress.CityId = request.CityId;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(existingAddress.Id);
        }
    }
}
