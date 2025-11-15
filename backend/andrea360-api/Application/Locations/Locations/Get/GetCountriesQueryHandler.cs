using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Locations.Get
{
    internal sealed class GetCountriesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetLocationsQuery, List<Location>>
    {
        public async Task<Result<List<Location>>> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            List<Location> result = await context.Locations.ToListAsync(cancellationToken);

            return result;
        }
    }
}
