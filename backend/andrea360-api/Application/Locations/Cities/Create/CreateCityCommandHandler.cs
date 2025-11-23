using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Locations.Cities.Get;
using Domain.Locations;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Cities.Create
{
    internal sealed class CreateAddressCommandHandler(IApplicationDbContext context, IMapper mapper) : ICommandHandler<CreateCityCommand, GetCityResponse>
    {
        public async Task<Result<GetCityResponse>> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            bool existing = await context.Cities.AnyAsync(c => c.Name == request.Name, cancellationToken);

            if (existing) return Result.Failure<GetCityResponse>
                           (new Error("City.NameTaken", $"The city name: '{request.Name} is taken'", ErrorType.Conflict));

            bool existingCountry = await context.Countries.AnyAsync(c => c.Id == request.CountryId, cancellationToken);

            if (existingCountry == false) return Result.Failure<GetCityResponse>
                    (new Error("Country.NotFound", $"The country with the id: '{request.CountryId}' was not found.", ErrorType.NotFound));

            var city = new City
            {
                Name = request.Name,
                CountryId = request.CountryId,
                IsDeleted = false
            };

            context.Cities.Add(city);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<GetCityResponse>(city));
        }
    }
}
