using Domain.Billing;
using Domain.Common;
using Domain.Scheduling;
using System;

namespace Domain.Catalog
{
    public class Service : BaseEntity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<UserService> UserServices { get; set; } = new List<UserService>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
