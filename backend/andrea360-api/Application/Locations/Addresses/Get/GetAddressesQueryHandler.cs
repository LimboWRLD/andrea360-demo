using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Addresses.Get
{
    internal sealed class GetAddressesQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetAddressesQuery, List<GetAddressResponse>>
    {
        public async Task<Result<List<GetAddressResponse>>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            List<Address> result = await context.Addresses.ToListAsync(cancellationToken);

            return Result.Success(mapper.Map<List<GetAddressResponse>>(result));
        }
    }
}
