using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.Update
{
    internal sealed class UpdateSessionCommandHandler(IApplicationDbContext context) : ICommandHandler<UpdateSessionCommand, Session>
    {
        public async Task<Result<Session>> Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
        {
            var existingSession = await context.Sessions.FindAsync(request.SessionId, cancellationToken);
            if (existingSession is null)
                return Result.Failure<Session>(new Error(
                    "Session.NotFound",
                    $"The session with id '{request.SessionId}' was not found.",
                    ErrorType.NotFound));

            bool locationExists = await context.Locations
                .AnyAsync(l => l.Id == request.LocationId, cancellationToken);

            if (!locationExists) return Result.Failure<Session>(new Error(
                "Session.NoLocation", $"Location with ID {request.LocationId} does not exist.", ErrorType.NotFound));

            bool serviceExists = await context.Services
                .AnyAsync(s => s.Id == request.ServiceId, cancellationToken);

            if (!serviceExists) return Result.Failure<Session>(new Error(
                "Session.NoService", $"Service with ID {request.ServiceId} does not exist.", ErrorType.NotFound));

            existingSession.StartTime = request.StartTime;
            existingSession.EndTime = request.EndTime;
            existingSession.LocationId = request.LocationId;
            existingSession.ServiceId = request.ServiceId;
            existingSession.MaxCapacity = request.MaxCapacity;
            existingSession.CurrentCapacity = request.CurrentCapacity;
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(existingSession);
        }
    }
}
