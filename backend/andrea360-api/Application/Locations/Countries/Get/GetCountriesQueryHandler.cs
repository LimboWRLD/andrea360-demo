using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Countries.Get
{
    internal sealed class GetCountriesQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetCountriesQuery, List<GetCountryResponse>>
    {
        public async Task<Result<List<GetCountryResponse>>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            List<Country> result = await context.Countries.Where(l => !l.IsDeleted).ToListAsync(cancellationToken);

            return Result.Success(mapper.Map<List<GetCountryResponse>>(result));
        }
    }
}
