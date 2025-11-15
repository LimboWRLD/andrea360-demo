using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.GetById
{
    internal sealed class GetSessionByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetSessionByIdQuery, Session>
    {
        public async Task<Result<Session>> Handle(GetSessionByIdQuery request, CancellationToken cancellationToken)
        {
            Session? session = await context.Sessions.FindAsync(request.SessionId, cancellationToken);
            if (session is null)
                return Result.Failure<Session>
                (new Error("Session.NotFound", $"The session with the Id='{request.SessionId}' was not found", ErrorType.NotFound));

            return Result.Success<Session>(session);
        }
    }
}
