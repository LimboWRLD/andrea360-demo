using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Billing.Transactions.Get;
using Domain.Billing;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.Transactions.GetByUserId
{
    internal sealed class GetTransactionsByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetTransactionsByUserIdQuery, List<GetTransactionResponse>>
    {
        public async Task<Result<List<GetTransactionResponse>>> Handle(GetTransactionsByUserIdQuery request, CancellationToken cancellationToken)
        {
            List<Transaction> result = await context.Transactions
                .Where(t => t.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            return Result.Success(mapper.Map<List<GetTransactionResponse>>(result));
        }
    }
}
