using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Countries.Delete
{
    internal sealed class DeleteLocationCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteCountryCommand>
    {
        public async Task<Result> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            Country? existingCountry = await context.Countries.FindAsync(request.CountryId, cancellationToken);

            if (existingCountry is null) return Result.Failure<Guid>
                    (new Error("Country.NotFound", $"The country with the id '{request.CountryId} was not found.'", ErrorType.NotFound));

            if (existingCountry.IsDeleted) Result.Failure<Guid>
                    (new Error("Country.AlreadyDeleted", $"The country with the id: '{request.CountryId}' is already deleted.", ErrorType.Conflict));

            existingCountry.IsDeleted = true;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
