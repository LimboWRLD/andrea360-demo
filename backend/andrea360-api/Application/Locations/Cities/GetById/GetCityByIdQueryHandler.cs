using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Cities.GetById
{
    internal sealed class GetCityByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetCityByIdQuery, City>
    {
        public async Task<Result<City>> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            City? city = await context.Cities.FindAsync(request.CityId, cancellationToken);

            if (city is null)
                return Result.Failure<City>
                    (new Error("City.NotFound", $"The city with the id: '{request.CityId}' was not found.", ErrorType.NotFound));

            return Result.Success<City>(city);
        }
    }
}
