using Application.Abstractions.Messaging;
using Application.Scheduling.Reservations.Get;
using Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Scheduling.Reservations.Create
{
    public class CreateReservationCommand : ICommand<GetReservationResponse>
    {
        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public Boolean IsCanceled { get; set; }
    }
}
