using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.Delete
{
    internal sealed class DeleteSessionCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteSessionCommand>
    {
        public async Task<Result> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
        {
            var existingSession = await context.Sessions.FindAsync(request.SessionId, cancellationToken);
            if (existingSession is null) return Result.Failure<Guid>
                    (new Error("Session.NotFound", $"The session with the id '{request.SessionId} was not found.'", ErrorType.NotFound));
            if (existingSession.IsDeleted) return Result.Failure<Guid>
                    (new Error("Session.AlreadyDeleted", $"The session with the id: '{request.SessionId}' is already deleted.", ErrorType.Conflict));
            existingSession.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
