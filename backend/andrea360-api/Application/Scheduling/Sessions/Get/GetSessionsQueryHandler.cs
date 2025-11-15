using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.Get
{
    internal sealed class GetSessionsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetSessionsQuery, List<Session>>
    {
        public async Task<Result<List<Session>>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
        {
            List<Session> result = await context.Sessions.ToListAsync(cancellationToken);
            return result;
        }
    }
}
