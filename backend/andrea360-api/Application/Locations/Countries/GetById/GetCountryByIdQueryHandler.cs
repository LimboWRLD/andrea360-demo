using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Locations.Countries.Get;
using Domain.Locations;
using MapsterMapper;

namespace Application.Locations.Countries.GetById
{
    internal sealed class GetCountryByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetCountryByIdQuery, GetCountryResponse>
    {
        public async Task<Result<GetCountryResponse>> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            Country? country = await context.Countries.FindAsync(request.CountryId, cancellationToken);

            if (country is null)
                return Result.Failure<GetCountryResponse>
                (new Error("Country.NotFound", $"The country with the Id='{request.CountryId}' was not found", ErrorType.NotFound));

            return Result.Success<GetCountryResponse>(mapper.Map<GetCountryResponse>(country));
        }
    }
}
