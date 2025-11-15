using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Addresses.Get
{
    internal sealed class GetAddressesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAddressesQuery, List<Address>>
    {
        public async Task<Result<List<Address>>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            List<Address> result = await context.Addresses.ToListAsync(cancellationToken);

            return result;
        }
    }
}
