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

namespace Application.Catalog.Services.GetById
{
    internal sealed class GetServiceByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetServiceByIdQuery, GetServiceResponse>
    {
        public async Task<Result<GetServiceResponse>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            Service? service = await context.Services.Where(l => !l.IsDeleted).FirstOrDefaultAsync(s => s.Id == request.ServiceId, cancellationToken);
            if (service is null)
                return Result.Failure<GetServiceResponse>
                (new Error("Service.NotFound", $"The service with the Id='{request.ServiceId}' was not found", ErrorType.NotFound));

            return Result.Success<GetServiceResponse>(mapper.Map<GetServiceResponse>(service));
        }
    }
}
