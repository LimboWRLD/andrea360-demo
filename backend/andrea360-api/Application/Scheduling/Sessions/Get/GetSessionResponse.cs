using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.Get
{
    public sealed class GetSessionResponse
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Guid LocationId { get; set; }
        public string LocationName { get; set; }  

        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }  
        public decimal ServicePrice { get; set; } 

        public int MaxCapacity { get; set; }
        public int CurrentCapacity { get; set; }
    }
}
