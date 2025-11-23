using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Scheduling.Reservations.Get;
using Domain.Scheduling;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.GetByUserId
{
    internal sealed class GetReservationByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetReservationByUserIdQuery, List<GetReservationResponse>>
    {
        public async Task<Result<List<GetReservationResponse>>> Handle(GetReservationByUserIdQuery request, CancellationToken cancellationToken)
        {
            List<Reservation> result = await context.Reservations
                .Where(r => r.UserId == request.UserId && !r.IsDeleted)
                .ToListAsync(cancellationToken);
            return Result.Success(mapper.Map<List<GetReservationResponse>>(result));
        }
    }
}
