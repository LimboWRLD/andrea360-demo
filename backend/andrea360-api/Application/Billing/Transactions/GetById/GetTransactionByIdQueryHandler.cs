using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Billing;


namespace Application.Billing.Transactions.GetById
{
    internal sealed class GetTransactionByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetTransactionByIdQuery, Transaction>
    {
        public async Task<Result<Transaction>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            Transaction? transaction = await context.Transactions.FindAsync(request.TransactionId, cancellationToken);

            if (transaction is null)
                return Result.Failure<Transaction>
                (new Error("Transaction.NotFound", $"The transaction with the Id='{request.TransactionId}' was not found", ErrorType.NotFound));

            return Result.Success<Transaction>(transaction);

        }
    }
}
