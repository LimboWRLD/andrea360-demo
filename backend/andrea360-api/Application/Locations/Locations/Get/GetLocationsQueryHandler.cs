using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Locations.Get
{
    internal sealed class GetLocationsQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetLocationsQuery, List<GetLocationResponse>>
    {
        public async Task<Result<List<GetLocationResponse>>> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            List<Location> result = await context.Locations.ToListAsync(cancellationToken);

            return Result.Success(mapper.Map<List<GetLocationResponse>>(result));
        }
    }
}
