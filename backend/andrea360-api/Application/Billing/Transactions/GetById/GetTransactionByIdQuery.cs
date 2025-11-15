using Application.Abstractions.Messaging;
using Domain.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Billing.Transactions.GetById;

public sealed record GetTransactionByIdQuery(Guid TransactionId) : IQuery<Transaction>;
