using Application.Abstractions.Messaging;
using Application.Billing.Transactions.Get;
using Domain.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Billing.Transactions.Update;

public sealed record UpdateTransactionCommand(Guid TransactionId , Guid UserId, Guid ServiceId, decimal Amount, DateTime TransactionDate, string StripeTransactionId) : ICommand<GetTransactionResponse>;
