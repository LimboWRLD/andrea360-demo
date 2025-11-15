using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.Transactions.Create
{
    internal sealed class CreateTransactionCommandHandler(IApplicationDbContext context)
        : ICommandHandler<CreateTransactionCommand, Transaction>
    {
        public async Task<Result<Transaction>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await context.BeginTransactionAsync(cancellationToken);

            try
            {
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

                var transactionEntity = new Transaction
                {
                    UserId = request.UserId,
                    ServiceId = request.ServiceId,
                    Amount = request.Amount,
                    TransactionDate = DateTime.UtcNow,
                    StripeTransactionId = request.StripeTransactionId
                };

                context.Transactions.Add(transactionEntity);

                await context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return Result.Success(transactionEntity);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
