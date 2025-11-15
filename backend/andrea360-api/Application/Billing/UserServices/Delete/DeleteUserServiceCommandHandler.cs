using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;

namespace Application.Billing.UserServices.Delete
{
    internal sealed class DeleteUserServiceCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteUserServiceCommand>
    {
        public async Task<Result> Handle(DeleteUserServiceCommand request, CancellationToken cancellationToken)
        {
            var existingUserService = await context.UserServices.FindAsync(request.UserServiceId, cancellationToken);
            if (existingUserService is null) return Result.Failure<Guid>
                    (new Error("UserService.NotFound", $"The user service with the id '{request.UserServiceId} was not found.'", ErrorType.NotFound));

            if (existingUserService.IsDeleted) return Result.Failure<Guid>
                    (new Error("UserService.AlreadyDeleted", $"The user service with the id: '{request.UserServiceId}' is already deleted.", ErrorType.Conflict));
            
            existingUserService.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
