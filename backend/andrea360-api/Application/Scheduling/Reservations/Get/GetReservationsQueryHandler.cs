using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Scheduling;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.Get
{
    internal sealed class GetReservationsQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetReservationsQuery, List<GetReservationResponse>>
    {
        public async Task<Result<List<GetReservationResponse>>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
        {
            List<Reservation> result = await context.Reservations.Where(l => !l.IsDeleted).ToListAsync(cancellationToken);
            return Result.Success(mapper.Map<List<GetReservationResponse>>(result));
        }
    }
}
