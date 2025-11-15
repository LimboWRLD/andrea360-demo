using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Billing.UserServices.Get;
using Domain.Billing;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Billing.UserServices.Update
{
    internal sealed class UpdateUserServiceCommandHandler(IApplicationDbContext context, IMapper mapper) : ICommandHandler<UpdateUserServiceCommand, GetUserServiceResponse>
    {
        public async Task<Result<GetUserServiceResponse>> Handle(UpdateUserServiceCommand request, CancellationToken cancellationToken)
        {
            var existingUserService = await context.UserServices.FindAsync(request.UserServiceId, cancellationToken);
            if (existingUserService is null) return Result.Failure<GetUserServiceResponse>(new Error(
                    "UserService.NotFound",
                    $"The user service with id '{request.UserServiceId}' was not found.",
                    ErrorType.NotFound));

            bool userExists = await context.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists) return Result.Failure<GetUserServiceResponse>(new Error(
                    "UserService.NoUser",
                    $"User with ID {request.UserId} does not exist.",
                    ErrorType.NotFound));

            bool serviceExists = await context.Services
                .AnyAsync(s => s.Id == request.ServiceId, cancellationToken);

            if (!serviceExists) return Result.Failure<GetUserServiceResponse>(new Error(
                    "UserService.NoService",
                    $"Service with ID {request.ServiceId} does not exist.",
                    ErrorType.NotFound));

            existingUserService.UserId = request.UserId;
            existingUserService.ServiceId = request.ServiceId;
            existingUserService.RemainingSessions = request.RemainingSessions;
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(mapper.Map<GetUserServiceResponse>(existingUserService));
        }
    }
}
