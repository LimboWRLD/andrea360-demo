using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Billing.UserServices.Create
{
    internal sealed class CreateUserServiceCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateUserServiceCommand, UserService>
    {
        public async Task<Result<UserService>> Handle(CreateUserServiceCommand request, CancellationToken cancellationToken)
        {
            bool userExists = await context.Users
                .AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!userExists)
            {
                return Result.Failure<UserService>(new Error(
                    "UserService.NoUser",
                    $"User with ID {request.UserId} does not exist.",
                    ErrorType.NotFound));
            }

            bool serviceExists = await context.Services
                .AnyAsync(s => s.Id == request.ServiceId, cancellationToken);

            if (!serviceExists)
            {
                return Result.Failure<UserService>(new Error(
                    "UserService.NoService",
                    $"Service with ID {request.ServiceId} does not exist.",
                    ErrorType.NotFound));
            }

            var existingUserService = await context.UserServices
                .FirstOrDefaultAsync(us => us.UserId == request.UserId && us.ServiceId == request.ServiceId, cancellationToken);

            if (existingUserService is null)
            {
                var newUserService = new UserService
                {
                    UserId = request.UserId,
                    ServiceId = request.ServiceId,
                    RemainingSessions = 1
                };

                context.UserServices.Add(newUserService);

                await context.SaveChangesAsync(cancellationToken);

                return Result.Success(newUserService);
            }

            existingUserService.RemainingSessions += 1;

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(existingUserService);
        }
    }
}
