using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Scheduling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.Get
{
    internal sealed class GetReservationsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetReservationsQuery, List<Reservation>>
    {
        public async Task<Result<List<Reservation>>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
        {
            List<Reservation> result = await context.Reservations.ToListAsync(cancellationToken);
            return Result.Success(result);
        }
    }
}
