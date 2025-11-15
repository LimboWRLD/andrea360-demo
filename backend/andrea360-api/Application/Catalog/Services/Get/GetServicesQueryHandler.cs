using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.Get
{
    internal sealed class GetServicesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetServicesQuery, List<Service>>
    {
        public async Task<Result<List<Service>>> Handle(GetServicesQuery request, CancellationToken cancellationToken)
        {
            List<Service> result = await context.Services.ToListAsync(cancellationToken);
            return Result.Success(result);
        }
    }
}
