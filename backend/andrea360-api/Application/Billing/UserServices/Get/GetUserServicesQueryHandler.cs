using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Billing;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.UserServices.Get
{
    internal sealed class GetUserServicesQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetUserServicesQuery, List<GetUserServiceResponse>>
    {
        public async Task<Result<List<GetUserServiceResponse>>> Handle(GetUserServicesQuery request, CancellationToken cancellationToken)
        {
            List<UserService> result = await context.UserServices.Where(l => !l.IsDeleted).ToListAsync(cancellationToken);
            return mapper.Map<List<GetUserServiceResponse>>(result);
        }
    }
}
