using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Billing.Transactions.Get;
using Domain.Billing;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.Transactions.Create
{
    internal sealed class CreateTransactionCommandHandler(IApplicationDbContext context, IMapper mapper)
        : ICommandHandler<CreateTransactionCommand, GetTransactionResponse>
    {
        public async Task<Result<GetTransactionResponse>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await context.BeginTransactionAsync(cancellationToken);

            try
            {
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

                return Result.Success(mapper.Map<GetTransactionResponse>(transactionEntity));
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
