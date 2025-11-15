using Application.Abstractions.Messaging;
using Domain.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Scheduling.Reservations.Create
{
    public class CreateReservationCommand : ICommand<Reservation>
    {
        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public Boolean IsCanceled { get; set; }
    }
}
