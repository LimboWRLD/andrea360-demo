using Application.Abstractions.Messaging;
using Application.Scheduling.Reservations.Get;
using Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.GetBySessionId;

public sealed record GetReservationBySessionIdQuery(Guid SessionId) : IQuery<List<GetReservationResponse>>;
