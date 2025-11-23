using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.Delete
{
    internal sealed class DeleteReservationCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteReservationCommand>
    {
        public async Task<Result> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var existingReservation = await context.Reservations.FindAsync(request.ReservationId, cancellationToken);
            if (existingReservation is null) return Result.Failure<Guid>
                    (new Error("Reservation.NotFound", $"The reservation with the id '{request.ReservationId} was not found.'", ErrorType.NotFound));
            if (existingReservation.IsDeleted) return Result.Failure<Guid>
                    (new Error("Reservation.AlreadyDeleted", $"The reservation with the id: '{request.ReservationId}' is already deleted.", ErrorType.Conflict));

            var existingSession = await context.Sessions.FindAsync(existingReservation.SessionId, cancellationToken);
            if (existingSession is not null && !existingSession.IsDeleted)
            {
                existingSession.CurrentCapacity -= 1;
            }
            existingReservation.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
