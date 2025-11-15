using Application.Abstractions.Messaging;
using Domain.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Billing.Transactions.Create
{
    public sealed class CreateTransactionCommand : ICommand<Transaction>
    {
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
        public decimal Amount { get; set; }
        public string StripeTransactionId { get; set; }
    }
}
