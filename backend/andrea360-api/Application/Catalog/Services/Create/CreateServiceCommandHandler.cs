using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Catalog.Services.Get;
using Domain.Catalog;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.Create
{
    internal sealed class CreateServiceCommandHandler(IApplicationDbContext context,IMapper mapper) : ICommandHandler<CreateServiceCommand, GetServiceResponse>
    {
        public async Task<Result<GetServiceResponse>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            bool nameExists = await context.Services
                .AnyAsync(s => s.Name == request.Name, cancellationToken);

            if (nameExists) return Result.Failure<GetServiceResponse>
                    (new Error("Service.NameExists",$"The service with the name '{request.Name}' already exists.", ErrorType.Conflict));

            var serviceEntity = new Service
            {
                Name = request.Name,
                Price = request.Price,
            };
            context.Services.Add(serviceEntity);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<GetServiceResponse>(serviceEntity));
        }
    }
}
