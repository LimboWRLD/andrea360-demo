using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Catalog;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.Get
{
    internal sealed class GetServicesQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetServicesQuery, List<GetServiceResponse>>
    {
        public async Task<Result<List<GetServiceResponse>>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
        {
            List<Service> result = await context.Services.Where(l => !l.IsDeleted).ToListAsync(cancellationToken);
            return Result.Success(mapper.Map<List<GetServiceResponse>>(result));
        }
    }
}
