using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Billing.Transactions.Get
{
    public sealed class GetTransactionResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
        public decimal Amount { get; set; }
        public string StripeTransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
