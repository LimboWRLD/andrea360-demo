using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Locations;

namespace Application.Locations.Addresses.Delete
{
    internal sealed class DeleteAddressCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteAddressCommand>
    {
        public async Task<Result> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            Address? existingAddress = await context.Addresses.FindAsync(request.AddressId, cancellationToken);

            if (existingAddress is null) return Result.Failure<Address>
                    (new Error("Address.NotFound", $"The Address with the id: '{request.AddressId}' was not found.", ErrorType.NotFound));

            if (existingAddress.IsDeleted) return Result.Failure<Guid>
                    (new Error("Address.AlreadyDeleted", $"The Address with the id: '{request.AddressId}' is already deleted.", ErrorType.Conflict));

            existingAddress.IsDeleted = true;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
