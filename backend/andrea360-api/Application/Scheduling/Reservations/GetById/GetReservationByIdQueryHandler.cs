using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Scheduling.Reservations.Get;
using Domain.Scheduling;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.GetById
{
    internal sealed class GetReservationByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetReservationByIdQuery, GetReservationResponse>
    {
        public async Task<Result<GetReservationResponse>> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            Reservation? reservation = await context.Reservations.FindAsync(request.ReservationId, cancellationToken);
            if (reservation is null) return Result.Failure<GetReservationResponse>
                (new Error("Reservation.NotFound", $"The reservation with the Id='{request.ReservationId}' was not found", ErrorType.NotFound));
            
            return Result.Success<GetReservationResponse>(mapper.Map<GetReservationResponse>(reservation));
        }
    }
}
