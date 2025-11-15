using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Countries.Create
{
    internal sealed class CreateCountryCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateCountryCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            bool exinsting = await context.Countries.AnyAsync(c => c.Name == request.Name);
            if (exinsting) return Result.Failure<Guid>
                           (new Error("Country.NameTaken", $"The country name: '{request.Name}' is taken", ErrorType.Conflict));

            var country = new Country
            {
                Name = request.Name,
                IsDeleted = false
            };

            context.Countries.Add(country);

            await context.SaveChangesAsync(cancellationToken);

            return country.Id;
        }
    }
}
