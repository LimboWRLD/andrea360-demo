using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Locations.Locations.Get;
using Domain.Locations;
using MapsterMapper;

namespace Application.Locations.Locations.GetById
{
    internal sealed class GetLocationByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetLocationByIdQuery, GetLocationResponse>
    {
        public async Task<Result<GetLocationResponse>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            Location? location = await context.Locations.FindAsync(request.LocationId, cancellationToken);

            if (location is null)
                return Result.Failure<GetLocationResponse>
                (new Error("Location.NotFound", $"The Location with the Id='{request.LocationId}' was not found", ErrorType.NotFound));

            return Result.Success<GetLocationResponse>(mapper.Map<GetLocationResponse>(location));
        }
    }
}
