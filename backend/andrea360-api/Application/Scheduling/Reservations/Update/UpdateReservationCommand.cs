using Application.Abstractions.Messaging;
using Application.Scheduling.Reservations.Get;
using Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.Update;

public sealed record UpdateReservationCommand(Guid ReservationId, Guid UserId, Guid SessionId, DateTime ReservedAt, Boolean IsCancelled) : ICommand<GetReservationResponse>;
