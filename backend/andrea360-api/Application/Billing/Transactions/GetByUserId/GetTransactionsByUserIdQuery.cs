using Application.Abstractions.Messaging;
using Application.Billing.Transactions.Get;
using Domain.Billing;

namespace Application.Billing.Transactions.GetByUserId;

public sealed record GetTransactionsByUserIdQuery(Guid UserId) : IQuery<List<GetTransactionResponse>>;
