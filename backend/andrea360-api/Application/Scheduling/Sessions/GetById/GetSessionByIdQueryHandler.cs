using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Scheduling.Sessions.Get;
using Domain.Scheduling;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.GetById
{
    internal sealed class GetSessionByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetSessionByIdQuery, GetSessionResponse>
    {
        public async Task<Result<GetSessionResponse>> Handle(GetSessionByIdQuery request, CancellationToken cancellationToken)
        {
            Session? session = await context.Sessions.Where(l => !l.IsDeleted).Include(s => s.Location).Include(s => s.Service).FirstOrDefaultAsync(s => s.Id == request.SessionId, cancellationToken);
            if (session is null)
                return Result.Failure<GetSessionResponse>
                (new Error("Session.NotFound", $"The session with the Id='{request.SessionId}' was not found", ErrorType.NotFound));

            return Result.Success<GetSessionResponse>(mapper.Map<GetSessionResponse>(session));
        }
    }
}
