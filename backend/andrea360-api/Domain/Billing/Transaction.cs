using Domain.Catalog;
using Domain.Common;

namespace Domain.Billing
{
    public class Transaction : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }

        public Service Service { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        public string StripeTransactionId { get; set; }
    }
}
