using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Countries.Update
{
    internal sealed class UpdateCountryCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateCountryCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            Country? existingCountry = await context.Countries.FindAsync(request.CountryId, cancellationToken);

            if (existingCountry is null) return Result.Failure<Guid>
                    (new Error("Country.NotFound", $"The country with the id '{request.CountryId} was not found.'", ErrorType.NotFound));

            bool sameName = await context.Countries.AnyAsync(c => c.Name == request.Name, cancellationToken);

            if (sameName) return Result.Failure<Guid>
                    (new Error("Country.NameTaken", $"The country name: '{request.Name}' is taken.", ErrorType.Conflict));

            if (request.Name == existingCountry.Name) return Result.Failure<Guid>
                    (new Error("Country.SameName", $"The country already has the name: '{request.Name}', try another name.", ErrorType.Conflict));
            existingCountry.Name = request.Name;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(existingCountry.Id);
        }
    }
}
