using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Delete
{
    internal sealed class DeleteUserCommandHandler(IApplicationDbContext context) : ICommandHandler<DeleteUserCommand>
    {
        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(request.UserId, cancellationToken);
            if (user == null)
                return Result.Failure(new Error("User.NotFound", "User not found.", ErrorType.NotFound));
            
            if (user.IsDeleted)
                return Result.Failure(new Error("User.AlreadyDeleted", "User is already deleted.", ErrorType.Conflict));
            
            user.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
