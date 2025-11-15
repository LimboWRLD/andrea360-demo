using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Billing.UserServices.Get;
using Domain.Billing;
using MapsterMapper;

namespace Application.Billing.UserServices.GetByUserId
{
    internal sealed class GetUserServiceByUserIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetUserServiceByUserIdQuery, List<GetUserServiceResponse>>
    {
        public async Task<Result<List<GetUserServiceResponse>>> Handle(GetUserServiceByUserIdQuery request, CancellationToken cancellationToken)
        {
            List<UserService> result = context.UserServices
                .Where(us => us.UserId == request.UserId)
                .ToList();
            return Result.Success<List<GetUserServiceResponse>>(mapper.Map<List<GetUserServiceResponse>>(result));
        }
    }
}
