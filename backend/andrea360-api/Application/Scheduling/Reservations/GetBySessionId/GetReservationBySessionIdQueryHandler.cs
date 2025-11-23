using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Scheduling.Reservations.Get;
using Application.Scheduling.Reservations.GetBySessionId;
using Domain.Scheduling;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.GetSessionById
{
    internal sealed class GetReservationBySessionIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetReservationBySessionIdQuery, List<GetReservationResponse>>
    {
        public async Task<Result<List<GetReservationResponse>>> Handle(GetReservationBySessionIdQuery request, CancellationToken cancellationToken)
        {
            var reservations = await context.Reservations
                .Where(r => !r.IsDeleted && r.SessionId == request.SessionId)
                .Include(r => r.User)
                .ToListAsync(cancellationToken);
            
            return Result.Success<List<GetReservationResponse>>(mapper.Map<List<GetReservationResponse>>(reservations));
        }
    }
}
