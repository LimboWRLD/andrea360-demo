using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Cities.Delete
{
    internal sealed class DeleteCityCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteCityCommand>
    {
        public async Task<Result> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            City? existingCity = await context.Cities.FindAsync(request.CityId, cancellationToken);

            if (existingCity is null) return Result.Failure<City>
                    (new Error("City.NotFound", $"The city with the id: '{request.CityId}' was not found.", ErrorType.NotFound));

            if (existingCity.IsDeleted) return Result.Failure<Guid>
                    (new Error("City.AlreadyDeleted", $"The city with the id: '{request.CityId}' is already deleted.", ErrorType.Conflict));

            existingCity.IsDeleted = true;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
