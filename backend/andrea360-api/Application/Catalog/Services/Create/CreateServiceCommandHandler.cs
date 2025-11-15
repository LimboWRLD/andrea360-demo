using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.Create
{
    internal sealed class CreateServiceCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateServiceCommand, Service>
    {
        public async Task<Result<Service>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            bool nameExists = await context.Services
                .AnyAsync(s => s.Name == request.Name, cancellationToken);

            if (nameExists) return Result.Failure<Service>
                    (new Error("Service.NameExists",$"The service with the name '{request.Name}' already exists.", ErrorType.Conflict));

            var serviceEntity = new Service
            {
                Name = request.Name,
                Price = request.Price,
            };
            context.Services.Add(serviceEntity);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(serviceEntity);
        }
    }
}
