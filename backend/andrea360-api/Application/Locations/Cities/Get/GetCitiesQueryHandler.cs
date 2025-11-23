using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Cities.Get
{
    internal sealed class GetCitiesQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetCitiesQuery, List<GetCityResponse>>
    {
        public async Task<Result<List<GetCityResponse>>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            List<City> result = await context.Cities.Include(c => c.Country).Where(l => !l.IsDeleted).ToListAsync(cancellationToken);

            return Result.Success(mapper.Map<List<GetCityResponse>>(result));
        }
    }
}
