using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Hubs;
using Application.Scheduling.Reservations.Get;
using Domain.Billing;
using Domain.Scheduling;
using MapsterMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.Create
{
    internal sealed class CreateReservationCommandHandler(IApplicationDbContext context, IMapper mapper, IHubContext<NotificationHub> hubContext) : ICommandHandler<CreateReservationCommand, GetReservationResponse>
    {
        public async Task<Result<GetReservationResponse>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            bool userExists = await context.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
                return Result.Failure<GetReservationResponse>(
                    new Error("Reservation.NoUser", $"User with ID {request.UserId} does not exist.", ErrorType.NotFound));

            var session = await context.Sessions
                .FirstOrDefaultAsync(s => s.Id == request.SessionId, cancellationToken);

            if (session is null)
                return Result.Failure<GetReservationResponse>(
                    new Error("Reservation.NoSession", $"Session with ID {request.SessionId} does not exist.", ErrorType.NotFound));

            UserService? canReserve = await context.UserServices
                .FirstOrDefaultAsync(us => us.UserId == request.UserId && us.ServiceId == session.ServiceId && us.RemainingSessions > 0, cancellationToken);

            if (canReserve is null)
                return Result.Failure<GetReservationResponse>(
                    new Error("Reservation.NoRemainingSessions", "User has no remaining sessions for this service.", ErrorType.Failure));

            bool alreadyReserved = await context.Reservations
                .AnyAsync(r => r.UserId == request.UserId && r.SessionId == request.SessionId, cancellationToken);

            if (alreadyReserved)
                return Result.Failure<GetReservationResponse>(
                    new Error("Reservation.AlreadyExists", "User already reserved this session", ErrorType.Conflict));

            if (session.CurrentCapacity >= session.MaxCapacity)
                return Result.Failure<GetReservationResponse>(
                    new Error("Reservation.Full", "Session is full.", ErrorType.Conflict));

            canReserve.RemainingSessions -= 1;

            var reservation = new Reservation
            {
                UserId = request.UserId,
                SessionId = request.SessionId,
                IsCancelled = false,
                ReservedAt = DateTime.UtcNow
            };

            context.Reservations.Add(reservation);
            session.CurrentCapacity += 1;

            await context.SaveChangesAsync(cancellationToken);

            await hubContext.Clients.All.SendAsync(
                "ReceiveCapacityUpdate",
                session.Id,
                session.CurrentCapacity,
                cancellationToken
            );


            return Result.Success(mapper.Map<GetReservationResponse>(reservation));
        }
    }
}
