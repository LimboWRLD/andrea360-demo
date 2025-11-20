using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Billing.Transactions.CreatePaymentIntent
{
    public sealed class CreatePaymentIntentCommand : ICommand<string>
    {
        public Guid ServiceId { get; set; }

        public Guid UserId { get; set; }
    }
}
