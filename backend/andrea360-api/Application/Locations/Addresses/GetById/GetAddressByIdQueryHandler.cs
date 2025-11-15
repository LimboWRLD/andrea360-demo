using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Locations.Addresses.Get;
using Domain.Locations;
using MapsterMapper;

namespace Application.Locations.Addresses.GetById
{
    internal sealed class GetAddressByIdQueryHandler(IApplicationDbContext context,IMapper mapper) : IQueryHandler<GetAddressByIdQuery, GetAddressResponse>
    {
        public async Task<Result<GetAddressResponse>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            Address? address = await context.Addresses.FindAsync(request.AddressId, cancellationToken);

            if (address is null)
                return Result.Failure<GetAddressResponse>
                    (new Error("Address.NotFound", $"The address with the id: '{request.AddressId}' was not found.", ErrorType.NotFound));

            return Result.Success<GetAddressResponse>(mapper.Map<GetAddressResponse>(address));
        }
    }
}
