using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Scheduling.Reservations.Get;
using Domain.Scheduling;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Scheduling.Reservations.Update
{
    internal sealed class UpdateReservationCommandHandler(IApplicationDbContext context, IMapper mapper)
        : ICommandHandler<UpdateReservationCommand, GetReservationResponse>
    {
        public async Task<Result<GetReservationResponse>> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            var existingReservation = await context.Reservations.FindAsync(request.ReservationId, cancellationToken);
            if (existingReservation is null)
            {
                return Result.Failure<GetReservationResponse>(new Error(
                    "Reservation.NotFound",
                    $"The reservation with id '{request.ReservationId}' was not found.",
                    ErrorType.NotFound));
            }

            var session = await context.Sessions.FindAsync(request.SessionId, cancellationToken);
            if (session is null)
            {
                return Result.Failure<GetReservationResponse>(new Error(
                    "Reservation.NoSession",
                    $"Session with ID {request.SessionId} does not exist.",
                    ErrorType.NotFound));
            }

            bool userExists = await context.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
            if (!userExists)
            {
                return Result.Failure<GetReservationResponse>(new Error(
                    "Reservation.NoUser",
                    $"User with ID {request.UserId} does not exist.",
                    ErrorType.NotFound));
            }

            if (session.CurrentCapacity >= session.MaxCapacity)
            {
                return Result.Failure<GetReservationResponse>(new Error(
                    "Reservation.Full",
                    "Reservation capacity is full.",
                    ErrorType.Failure));
            }

            existingReservation.SessionId = request.SessionId;
            existingReservation.ReservedAt = request.ReservedAt;
            existingReservation.UserId = request.UserId;
            existingReservation.IsCancelled = request.IsCancelled;

            session.CurrentCapacity++;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(mapper.Map<GetReservationResponse>(existingReservation));
        }
    }
}
