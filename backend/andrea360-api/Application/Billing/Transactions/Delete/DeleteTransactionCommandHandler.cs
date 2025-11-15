using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;

namespace Application.Billing.Transactions.Delete
{
    internal sealed class DeleteTransactionCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteTransactionCommand>
    {
        public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var existingTransaction = await context.Transactions.FindAsync(request.TransactionId, cancellationToken);

            if (existingTransaction is null) return Result.Failure<Guid>
                    (new Error("Transaction.NotFound", $"The transaction with the id '{request.TransactionId} was not found.'", ErrorType.NotFound));   

            if (existingTransaction.IsDeleted) return Result.Failure<Guid>
                    (new Error("Transaction.AlreadyDeleted", $"The transaction with the id: '{request.TransactionId}' is already deleted.", ErrorType.Conflict));

            existingTransaction.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
