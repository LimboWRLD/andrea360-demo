using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.Create
{
    internal sealed class CreateSessionCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateSessionCommand, Session>
    {
        public async Task<Result<Session>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            bool locationExists = await context.Locations
                .AnyAsync(l => l.Id == request.LocationId, cancellationToken);

            if (!locationExists) return Result.Failure<Session>(
                new Error("Session.InvalidLocation", $"The location with id '{request.LocationId}' does not exist.", ErrorType.NotFound));

            bool serviceExists = await context.Services
                .AnyAsync(s => s.Id == request.ServiceId, cancellationToken);

            if (!serviceExists) return Result.Failure<Session>(
                new Error("Session.InvalidService", $"The service with id '{request.ServiceId}' does not exist.", ErrorType.NotFound));

            var session = new Session
            {
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                LocationId = request.LocationId,
                ServiceId = request.ServiceId,
                MaxCapacity = request.MaxCapacity,
                CurrentCapacity = 0
            };
            context.Sessions.Add(session);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(session);
        }
    }
}
