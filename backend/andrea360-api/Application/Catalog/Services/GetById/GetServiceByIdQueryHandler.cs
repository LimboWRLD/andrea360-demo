using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.GetById
{
    internal sealed class GetServiceByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetServiceByIdQuery, Service>
    {
        public async Task<Result<Service>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            Service? service = await context.Services.FindAsync(new object[] { request.ServiceId }, cancellationToken);
            if (service is null)
                return Result.Failure<Service>
                (new Error("Service.NotFound", $"The service with the Id='{request.ServiceId}' was not found", ErrorType.NotFound));

            return Result.Success<Service>(service);
        }
    }
}
