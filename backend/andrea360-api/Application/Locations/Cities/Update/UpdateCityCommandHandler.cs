using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Locations.Cities.Get;
using Domain.Locations;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Cities.Update
{
    internal sealed class UpdateCityCommandHandler(IApplicationDbContext context, IMapper mapper) : ICommandHandler<UpdateCityCommand, GetCityResponse>
    {
        public async Task<Result<GetCityResponse>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            City? existingCity = await context.Cities.FindAsync(request.CityId, cancellationToken);

            if (existingCity is null) return Result.Failure<GetCityResponse>
                    (new Error("City.NotFound", $"The city with the id: '{request.CityId}' was not found.", ErrorType.NotFound));


            bool sameName = await context.Cities.AnyAsync(c => c.Name == request.Name && c.CountryId == request.CountryId, cancellationToken);
            if (sameName) Result.Failure<GetCityResponse>
                    (new Error("City.NameTaken", $"The city name: '{request.Name}' is taken.", ErrorType.Conflict));

            existingCity.Name = request.Name;
            existingCity.CountryId = request.CountryId;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<GetCityResponse>(existingCity));
        }
    }
}
