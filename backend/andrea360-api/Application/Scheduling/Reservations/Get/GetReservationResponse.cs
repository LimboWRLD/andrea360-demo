using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.Get
{
    public sealed class GetReservationResponse
    {
        public Guid Id { get; set; }  
        public Guid SessionId { get; set; }
        public DateTime ReservedAt { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }  
        public bool IsCancelled { get; set; }
    }
}
