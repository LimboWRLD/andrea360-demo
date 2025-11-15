using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Billing;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.Transactions.Get
{
    internal sealed class GetTransactionsQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetTransactionsQuery, List<GetTransactionResponse>>
    {
        public async Task<Result<List<GetTransactionResponse>>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            List<Transaction> result = await context.Transactions.ToListAsync(cancellationToken);

            return mapper.Map<List<GetTransactionResponse>>(result);
        }
    }
}
