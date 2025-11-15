using Application.Abstractions.Messaging;

namespace Application.Billing.Transactions.Delete;

public sealed record DeleteTransactionCommand(Guid TransactionId) : ICommand;