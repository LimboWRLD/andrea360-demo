using Application.Abstractions.Messaging;
using Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.Update;

public sealed record UpdateSessionCommand(Guid SessionId, DateTime StartTime, DateTime EndTime, Guid LocationId, Guid ServiceId, int MaxCapacity, int CurrentCapacity) : ICommand<Session>;
