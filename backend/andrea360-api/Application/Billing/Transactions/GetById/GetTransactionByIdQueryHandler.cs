using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Billing.Transactions.Get;
using Domain.Billing;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;


namespace Application.Billing.Transactions.GetById
{
    internal sealed class GetTransactionByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetTransactionByIdQuery, GetTransactionResponse>
    {
        public async Task<Result<GetTransactionResponse>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            Transaction? transaction = await context.Transactions.Where(l => !l.IsDeleted).FirstOrDefaultAsync(t => t.Id == request.TransactionId, cancellationToken);

            if (transaction is null)
                return Result.Failure<GetTransactionResponse>
                (new Error("Transaction.NotFound", $"The transaction with the Id='{request.TransactionId}' was not found", ErrorType.NotFound));

            return Result.Success<GetTransactionResponse>(mapper.Map<GetTransactionResponse>(transaction));

        }
    }
}
