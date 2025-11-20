using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Scheduling;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.Get
{
    internal sealed class GetSessionsQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetSessionsQuery, List<GetSessionResponse>>
    {
        public async Task<Result<List<GetSessionResponse>>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
        {
            List<Session> result = await context.Sessions.Where(l => !l.IsDeleted).ToListAsync(cancellationToken);
            return Result.Success(mapper.Map<List<GetSessionResponse>>(result));
        }
    }
}
