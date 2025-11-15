using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.Transactions.Get
{
    internal sealed class GetTransactionsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetTransactionsQuery, List<Transaction>>
    {
        public async Task<Result<List<Transaction>>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            List<Transaction> result = await context.Transactions.ToListAsync(cancellationToken);

            return result;
        }
    }
}
