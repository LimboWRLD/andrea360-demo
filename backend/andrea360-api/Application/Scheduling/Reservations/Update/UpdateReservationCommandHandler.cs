using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Hubs;
using Application.Scheduling.Reservations.Get;
using MapsterMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Application.Scheduling.Reservations.Update
{
    internal sealed class UpdateReservationCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IHubContext<NotificationHub> hubContext
    ) : ICommandHandler<UpdateReservationCommand, GetReservationResponse>
    {
        public async Task<Result<GetReservationResponse>> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            var existingReservation = await context.Reservations
                .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);

            if (existingReservation is null)
                return Result.Failure<GetReservationResponse>(
                    new Error("Reservation.NotFound", $"Reservation {request.ReservationId} not found.", ErrorType.NotFound));

            var newSession = await context.Sessions
                .FirstOrDefaultAsync(s => s.Id == request.SessionId, cancellationToken);

            if (newSession is null)
                return Result.Failure<GetReservationResponse>(
                    new Error("Reservation.NoSession", $"Session {request.SessionId} does not exist.", ErrorType.NotFound));

            bool userExists = await context.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
                return Result.Failure<GetReservationResponse>(
                    new Error("Reservation.NoUser", $"User {request.UserId} does not exist.", ErrorType.NotFound));

            bool alreadyReserved = await context.Reservations
                .AnyAsync(r => r.UserId == request.UserId
                               && r.SessionId == request.SessionId
                               && r.Id != request.ReservationId, cancellationToken);

            if (alreadyReserved)
                return Result.Failure<GetReservationResponse>(
                    new Error("Reservation.Exists", "User already has reservation for this session.", ErrorType.Conflict));

            var userService = await context.UserServices
                .FirstOrDefaultAsync(us => us.UserId == request.UserId
                                           && us.ServiceId == newSession.ServiceId, cancellationToken);

            if (userService is null)
                return Result.Failure<GetReservationResponse>(
                    new Error("Reservation.NoRemainingSessions", "User has no remaining sessions for this service.", ErrorType.Failure));

            var oldSessionId = existingReservation.SessionId;
            var oldSession = await context.Sessions
                .FirstOrDefaultAsync(s => s.Id == oldSessionId, cancellationToken);

            var oldUserService = oldSession != null
                ? await context.UserServices
                    .FirstOrDefaultAsync(us => us.UserId == request.UserId && us.ServiceId == oldSession.ServiceId, cancellationToken)
                : null;

            bool wasCancelled = existingReservation.IsCancelled;
            bool isMovingToNewSession = oldSessionId != request.SessionId;
            bool isBecomingActive = wasCancelled && !request.IsCancelled;

            if ((isMovingToNewSession || isBecomingActive) && newSession.CurrentCapacity >= newSession.MaxCapacity)
                return Result.Failure<GetReservationResponse>(
                    new Error("Reservation.Full", "Session is full.", ErrorType.Conflict));

            if (isMovingToNewSession)
            {
                if (oldSession != null)
                {
                    oldSession.CurrentCapacity--;
                    if (oldUserService != null) oldUserService.RemainingSessions++;

                    await hubContext.Clients.All.SendAsync(
                        "ReceiveCapacityUpdate",
                        oldSession.Id,
                        oldSession.CurrentCapacity,
                        cancellationToken
                    );
                }

                userService.RemainingSessions--;
                newSession.CurrentCapacity++;
            }
            else if (wasCancelled != request.IsCancelled)
            {
                if (request.IsCancelled)
                {
                    newSession.CurrentCapacity--;
                    userService.RemainingSessions++;
                }
                else
                {
                    newSession.CurrentCapacity++;
                    userService.RemainingSessions--;
                }
            }

            existingReservation.SessionId = request.SessionId;
            existingReservation.UserId = request.UserId;
            existingReservation.ReservedAt = request.ReservedAt;
            existingReservation.IsCancelled = request.IsCancelled;

            await context.SaveChangesAsync(cancellationToken);


            await hubContext.Clients.All.SendAsync(
                "ReceiveCapacityUpdate",
                newSession.Id,
                newSession.CurrentCapacity,
                cancellationToken
            );

            return Result.Success(mapper.Map<GetReservationResponse>(existingReservation));
        }
    }
}
