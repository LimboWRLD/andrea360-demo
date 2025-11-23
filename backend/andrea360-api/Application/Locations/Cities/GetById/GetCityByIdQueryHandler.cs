using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Locations.Cities.Get;
using Domain.Locations;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Cities.GetById
{
    internal sealed class GetCityByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetCityByIdQuery, GetCityResponse>
    {
        public async Task<Result<GetCityResponse>> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            City? city = await context.Cities.Include(c => c.Country).Where(l => !l.IsDeleted).FirstOrDefaultAsync(c => c.Id == request.CityId, cancellationToken);

            if (city is null)
                return Result.Failure<GetCityResponse>
                    (new Error("City.NotFound", $"The city with the id: '{request.CityId}' was not found.", ErrorType.NotFound));

            return Result.Success<GetCityResponse>(mapper.Map<GetCityResponse>(city));
        }
    }
}
