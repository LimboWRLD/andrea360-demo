using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Billing.UserServices.Get;
using Domain.Billing;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.UserServices.GetById
{
    internal sealed class GetUserServiceByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IQueryHandler<GetUserServiceByIdQuery, GetUserServiceResponse>
    {
        public async Task<Result<GetUserServiceResponse>> Handle(GetUserServiceByIdQuery request, CancellationToken cancellationToken)
        {
            UserService? userService = await context.UserServices.Where(l => !l.IsDeleted).FirstOrDefaultAsync(us => us.Id == request.UserServiceId, cancellationToken);
            if (userService is null)
                return Result.Failure<GetUserServiceResponse>
                (new Error("UserService.NotFound", $"The user service with the Id='{request.UserServiceId}' was not found", ErrorType.NotFound));

            return Result.Success<GetUserServiceResponse>(mapper.Map<GetUserServiceResponse>(userService));
        }
    }
}
