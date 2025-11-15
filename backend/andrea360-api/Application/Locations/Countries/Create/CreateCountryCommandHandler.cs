using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Locations.Countries.Get;
using Domain.Locations;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Locations.Countries.Create
{
    internal sealed class CreateCountryCommandHandler(IApplicationDbContext context, IMapper mapper) : ICommandHandler<CreateCountryCommand, GetCountryResponse>
    {
        public async Task<Result<GetCountryResponse>> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            bool exinsting = await context.Countries.AnyAsync(c => c.Name == request.Name);
            if (exinsting) return Result.Failure<GetCountryResponse>
                           (new Error("Country.NameTaken", $"The country name: '{request.Name}' is taken", ErrorType.Conflict));

            var country = new Country
            {
                Name = request.Name,
                IsDeleted = false
            };

            context.Countries.Add(country);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<GetCountryResponse>(country));
        }
    }
}
