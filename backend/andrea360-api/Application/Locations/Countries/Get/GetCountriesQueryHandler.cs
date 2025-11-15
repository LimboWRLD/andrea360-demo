using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Countries.Get
{
    internal sealed class GetCountriesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetCountriesQuery, List<Country>>
    {
        public async Task<Result<List<Country>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            List<Country> result = await context.Countries.ToListAsync(cancellationToken);

            return result;
        }
    }
}
