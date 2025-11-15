using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.GetById
{
    internal sealed class GetReservationByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetReservationByIdQuery, Reservation>
    {
        public async Task<Result<Reservation>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            Reservation? reservation = await context.Reservations.FindAsync(request.ReservationId, cancellationToken);
            if (reservation is null) return Result.Failure<Reservation>
                (new Error("Reservation.NotFound", $"The reservation with the Id='{request.ReservationId}' was not found", ErrorType.NotFound));
            
            return Result.Success<Reservation>(reservation);
        }
    }
}
