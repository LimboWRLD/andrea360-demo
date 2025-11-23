using Application.Abstractions.Messaging;
using Application.Scheduling.Sessions.Get;
using Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.GetById;

public sealed record GetSessionByIdQuery(Guid SessionId) : IQuery<GetSessionResponse>;
