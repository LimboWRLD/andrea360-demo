using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Addresses.Create
{
    internal sealed class CreateOrderCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateAddressCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            bool existing = await context.Addresses.AnyAsync(c => c.Street == request.Street && c.Number == request.Number, cancellationToken);

            if (existing) return Result.Failure<Guid>
                           (new Error("Address.Taken", $"The address: '{request.Street}' number '{request.Number}' is taken", ErrorType.Conflict));

            bool existingCity = await context.Cities.AnyAsync(c => c.Id == request.CityId, cancellationToken);

            if (existingCity == false) return Result.Failure<Guid>
                    (new Error("City.NotFound", $"The city with the id: '{request.CityId}' was not found.", ErrorType.NotFound));

            var address = new Address
            {
                Street = request.Street,
                Number = request.Number,
                CityId = request.CityId,
                IsDeleted = false
            };

            context.Addresses.Add(address);

            await context.SaveChangesAsync(cancellationToken);

            return address.Id;
        }
    }
}
