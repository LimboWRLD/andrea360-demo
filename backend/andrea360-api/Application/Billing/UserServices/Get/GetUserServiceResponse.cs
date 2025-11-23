using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Billing.UserServices.Get
{
    public sealed class GetUserServiceResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }

        public string? ServiceName { get; set; }
        public int RemainingSessions { get; set; }
    }
}
