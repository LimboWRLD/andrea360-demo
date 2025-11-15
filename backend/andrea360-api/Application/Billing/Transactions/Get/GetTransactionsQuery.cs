using Application.Abstractions.Messaging;
using Domain.Billing;

namespace Application.Billing.Transactions.Get;

public sealed record GetTransactionsQuery : IQuery<List<GetTransactionResponse>>;