using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.UserServices.Get
{
    internal sealed class GetUserServicesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetUserServicesQuery, List<UserService>>
    {
        public async Task<Result<List<UserService>>> Handle(GetUserServicesQuery request, CancellationToken cancellationToken)
        {
            List<UserService> result = await context.UserServices.ToListAsync(cancellationToken);
            return result;
        }
    }
}
