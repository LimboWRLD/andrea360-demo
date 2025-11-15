using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Billing;

namespace Application.Billing.UserServices.GetByUserId
{
    internal sealed class GetUserServiceByUserIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetUserServiceByUserIdQuery, List<UserService>>
    {
        public async Task<Result<List<UserService>>> Handle(GetUserServiceByUserIdQuery request, CancellationToken cancellationToken)
        {
            List<UserService> result = context.UserServices
                .Where(us => us.UserId == request.UserId)
                .ToList();
            return Result.Success<List<UserService>>(result);
        }
    }
}
