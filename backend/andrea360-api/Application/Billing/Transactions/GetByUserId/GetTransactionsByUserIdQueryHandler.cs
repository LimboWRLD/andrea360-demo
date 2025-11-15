using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.Transactions.GetByUserId
{
    internal sealed class GetTransactionsByUserIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetTransactionsByUserIdQuery, List<Transaction>>
    {
        public async Task<Result<List<Transaction>>> Handle(GetTransactionsByUserIdQuery request, CancellationToken cancellationToken)
        {
            List<Transaction> result = await context.Transactions
                .Where(t => t.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            return Result.Success(result);
        }
    }
}
