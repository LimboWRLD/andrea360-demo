using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.Delete
{
    internal sealed class DeleteServiceCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteServiceCommand>
    {
        public async Task<Result> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var existingService = await context.Services.FindAsync(request.ServiceId, cancellationToken);
            if (existingService is null) return Result.Failure<Guid>
                    (new Error("Service.NotFound", $"The service with the id '{request.ServiceId} was not found.'", ErrorType.NotFound));

            if (existingService.IsDeleted) return Result.Failure<Guid>
                    (new Error("Service.AlreadyDeleted", $"The service with the id: '{request.ServiceId}' is already deleted.", ErrorType.Conflict));

            existingService.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
