using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Billing.Transactions.Get;
using Domain.Billing;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.Transactions.Update
{
    internal sealed class UpdateTransactionCommandHandler(IApplicationDbContext context, IMapper mapper)
        : ICommandHandler<UpdateTransactionCommand, GetTransactionResponse>
    {
        public async Task<Result<GetTransactionResponse>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await context.BeginTransactionAsync(cancellationToken);

            try
            {
                var existingTransaction = await context.Transactions
                    .FindAsync([request.TransactionId], cancellationToken);

                if (existingTransaction is null)
                    return Result.Failure<GetTransactionResponse>(new Error(
                        "Transaction.NotFound",
                        $"The transaction with id '{request.TransactionId}' was not found.",
                        ErrorType.NotFound));

                bool userExists = await context.Users
                    .AnyAsync(u => u.Id == request.UserId, cancellationToken);

                if (!userExists)
                    return Result.Failure<GetTransactionResponse>(new Error(
                        "Transaction.NoUser",
                        $"User with ID {request.UserId} does not exist.",
                        ErrorType.NotFound));

                bool serviceExists = await context.Services
                    .AnyAsync(s => s.Id == request.ServiceId, cancellationToken);

                if (!serviceExists)
                    return Result.Failure<GetTransactionResponse>(new Error(
                        "Transaction.NoService",
                        $"Service with ID {request.ServiceId} does not exist.",
                        ErrorType.NotFound));

                existingTransaction.UserId = request.UserId;
                existingTransaction.ServiceId = request.ServiceId;
                existingTransaction.Amount = request.Amount;
                existingTransaction.TransactionDate = request.TransactionDate;

                await context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return Result.Success(mapper.Map<GetTransactionResponse>(existingTransaction));
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
