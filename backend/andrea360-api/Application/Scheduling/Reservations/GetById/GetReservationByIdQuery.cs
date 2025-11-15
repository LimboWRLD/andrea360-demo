using Application.Abstractions.Messaging;
using Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.GetById;

public sealed record GetReservationByIdQuery(Guid ReservationId) : IQuery<Reservation>;
