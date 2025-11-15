using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Cities.Get
{
    internal sealed class GetCitiesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetCitiesQuery, List<City>>
    {
        public async Task<Result<List<City>>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            List<City> result = await context.Cities.ToListAsync(cancellationToken);

            return result;
        }
    }
}
