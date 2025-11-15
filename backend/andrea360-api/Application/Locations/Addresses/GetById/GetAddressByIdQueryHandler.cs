using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Addresses.GetById
{
    internal sealed class GetAddressByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAddressByIdQuery, Address>
    {
        public async Task<Result<Address>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            Address? address = await context.Addresses.FindAsync(request.AddressId, cancellationToken);

            if (address is null)
                return Result.Failure<Address>
                    (new Error("Address.NotFound", $"The address with the id: '{request.AddressId}' was not found.", ErrorType.NotFound));

            return Result.Success<Address>(address);
        }
    }
}
