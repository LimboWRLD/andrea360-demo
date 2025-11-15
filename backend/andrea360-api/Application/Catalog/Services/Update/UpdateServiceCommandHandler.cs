using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.Update
{
    internal sealed class UpdateServiceCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateServiceCommand, Service>
    {
        public async Task<Result<Service>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var existingService = await context.Services.FindAsync(request.ServiceId, cancellationToken);
            if (existingService is null)
            {
                return Result.Failure<Service>(new Error(
                    "Service.NotFound",
                    $"The service with id '{request.ServiceId}' was not found.",
                    ErrorType.NotFound));
            }

            existingService.Name = request.Name;
            existingService.Price = request.Price;
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(existingService);
        }
    }
}
