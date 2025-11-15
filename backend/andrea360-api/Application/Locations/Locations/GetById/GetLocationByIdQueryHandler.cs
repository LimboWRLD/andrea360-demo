using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Locations.GetById
{
    internal sealed class GetLocationByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetLocationByIdQuery, Location>
    {
        public async Task<Result<Location>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            Location? location = await context.Locations.FindAsync(request.LocationId, cancellationToken);

            if (location is null)
                return Result.Failure<Location>
                (new Error("Location.NotFound", $"The Location with the Id='{request.LocationId}' was not found", ErrorType.NotFound));

            return Result.Success<Location>(location);
        }
    }
}
