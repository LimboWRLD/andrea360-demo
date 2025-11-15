using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.Transactions.Update
{
    internal sealed class UpdateTransactionCommandHandler(IApplicationDbContext context)
        : ICommandHandler<UpdateTransactionCommand, Transaction>
    {
        public async Task<Result<Transaction>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await context.BeginTransactionAsync(cancellationToken);

            try
            {
                var existingTransaction = await context.Transactions
                    .FindAsync([request.TransactionId], cancellationToken);

                if (existingTransaction is null)
                    return Result.Failure<Transaction>(new Error(
                        "Transaction.NotFound",
                        $"The transaction with id '{request.TransactionId}' was not found.",
                        ErrorType.NotFound));

                bool userExists = await context.Users
                    .AnyAsync(u => u.Id == request.UserId, cancellationToken);

                if (!userExists)
                    return Result.Failure<Transaction>(new Error(
                        "Transaction.NoUser",
                        $"User with ID {request.UserId} does not exist.",
                        ErrorType.NotFound));

                bool serviceExists = await context.Services
                    .AnyAsync(s => s.Id == request.ServiceId, cancellationToken);

                if (!serviceExists)
                    return Result.Failure<Transaction>(new Error(
                        "Transaction.NoService",
                        $"Service with ID {request.ServiceId} does not exist.",
                        ErrorType.NotFound));

                existingTransaction.UserId = request.UserId;
                existingTransaction.ServiceId = request.ServiceId;
                existingTransaction.Amount = request.Amount;
                existingTransaction.TransactionDate = request.TransactionDate;

                await context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return Result.Success(existingTransaction);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
