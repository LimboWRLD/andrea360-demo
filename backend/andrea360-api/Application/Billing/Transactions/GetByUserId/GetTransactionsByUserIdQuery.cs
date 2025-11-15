using Application.Abstractions.Messaging;
using Domain.Billing;

namespace Application.Billing.Transactions.GetByUserId;

public sealed record GetTransactionsByUserIdQuery(Guid UserId) : IQuery<List<Transaction>>;
