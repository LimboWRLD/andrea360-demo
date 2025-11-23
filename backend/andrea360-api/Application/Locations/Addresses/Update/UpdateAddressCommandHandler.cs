using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Locations.Addresses.Get;
using Domain.Locations;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Addresses.Update
{
    internal sealed class UpdateOrderCommandHandler(IApplicationDbContext context, IMapper mapper) : ICommandHandler<UpdateAddressCommand, GetAddressResponse>
    {
        public async Task<Result<GetAddressResponse>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            Address? existingAddress = await context.Addresses.FindAsync(request.AddressId, cancellationToken);

            if (existingAddress is null) return Result.Failure<GetAddressResponse>
                    (new Error("Address.NotFound", $"The address with the id: '{request.AddressId}' was not found.", ErrorType.NotFound));


            bool sameStreetAndNumber = await context.Addresses.AnyAsync(c => c.Number == request.Number && c.Street == request.Street && c.CityId == request.CityId, cancellationToken);
            if (sameStreetAndNumber) Result.Failure<GetAddressResponse>
                    (new Error("Address.Taken", $"The address : '{request.Street}' number : '{request.Number}' is taken.", ErrorType.Conflict));

            existingAddress.Street = request.Street;
            existingAddress.Number = request.Number;
            existingAddress.CityId = request.CityId;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<GetAddressResponse>(existingAddress));
        }
    }
}
