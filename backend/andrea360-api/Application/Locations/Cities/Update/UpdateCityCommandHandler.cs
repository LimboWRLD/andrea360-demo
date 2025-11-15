using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Cities.Update
{
    internal sealed class UpdateCityCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateCityCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            City? existingCity = await context.Cities.FindAsync(request.CityId, cancellationToken);

            if (existingCity is null) return Result.Failure<Guid>
                    (new Error("City.NotFound", $"The city with the id: '{request.CityId}' was not found.", ErrorType.NotFound));


            bool sameName = await context.Cities.AnyAsync(c => c.Name == request.Name && c.CountryId == request.CountryId, cancellationToken);
            if (sameName) Result.Failure<Guid>
                    (new Error("City.NameTaken", $"The city name: '{request.Name}' is taken.", ErrorType.Conflict));

            existingCity.Name = request.Name;
            existingCity.CountryId = request.CountryId;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(existingCity.Id);
        }
    }
}
