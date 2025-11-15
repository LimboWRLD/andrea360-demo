using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Countries.GetById
{
    internal sealed class GetCountryByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetCountryByIdQuery, Country>
    {
        public async Task<Result<Country>> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            Country? country = await context.Countries.FindAsync(request.CountryId, cancellationToken);

            if (country is null)
                return Result.Failure<Country>
                (new Error("Country.NotFound", $"The country with the Id='{request.CountryId}' was not found", ErrorType.NotFound));

            return Result.Success<Country>(country);
        }
    }
}
