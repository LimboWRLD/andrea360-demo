using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Billing;

namespace Application.Billing.UserServices.GetById
{
    internal sealed class GetUserServiceByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetUserServiceByIdQuery, UserService>
    {
        public async Task<Result<UserService>> Handle(GetUserServiceByIdQuery request, CancellationToken cancellationToken)
        {
            UserService? userService = await context.UserServices.FindAsync(request.UserServiceId, cancellationToken);
            if (userService is null)
                return Result.Failure<UserService>
                (new Error("UserService.NotFound", $"The user service with the Id='{request.UserServiceId}' was not found", ErrorType.NotFound));

            return Result.Success<UserService>(userService);
        }
    }
}
