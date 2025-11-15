using Application.Abstractions.Messaging;
using Application.Scheduling.Sessions.Get;
using Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.Create
{
    public class CreateSessionCommand : ICommand<GetSessionResponse>
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Guid LocationId { get; set; }

        public Guid ServiceId { get; set; }

        public int MaxCapacity { get; set; }
    }
}
