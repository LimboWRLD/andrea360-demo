using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.Create
{
    internal sealed class CreateReservationCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateReservationCommand, Reservation>
    {
        public async Task<Result<Reservation>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            bool userExists = await context.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists) return Result.Failure<Reservation>(new Error(
                "Reservation.NoUser", $"User with ID {request.UserId} does not exist.", ErrorType.NotFound));

            Session? sessionExists = await context.Sessions
                .FindAsync(request.SessionId, cancellationToken);

            if (sessionExists is null) return Result.Failure<Reservation>(new Error(
                "Reservation.NoSession", $"Session with ID {request.SessionId} does not exist.", ErrorType.NotFound));

            var reservation = new Reservation
            {
                UserId = request.UserId,
                SessionId = request.SessionId,
                IsCancelled = false,
                ReservedAt = DateTime.UtcNow
            };
            context.Reservations.Add(reservation);

            sessionExists.CurrentCapacity += 1;
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(reservation);
        }
    }
}
