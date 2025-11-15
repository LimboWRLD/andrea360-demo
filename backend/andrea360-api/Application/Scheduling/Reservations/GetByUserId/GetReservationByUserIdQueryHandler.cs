using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.GetByUserId
{
    internal sealed class GetReservationByUserIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetReservationByUserIdQuery, List<Reservation>>
    {
        public async Task<Result<List<Reservation>>> Handle(GetReservationByUserIdQuery request, CancellationToken cancellationToken)
        {
            List<Reservation> result = await context.Reservations
                .Where(r => r.UserId == request.UserId)
                .ToListAsync(cancellationToken);
            return Result.Success(result);
        }
    }
}
