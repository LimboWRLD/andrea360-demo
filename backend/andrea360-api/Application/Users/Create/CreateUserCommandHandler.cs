using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Create
{
    internal sealed class CreateUserCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateUserCommand, User>
    {
        public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool emailExists = context.Users.Any(u => u.Email == request.Email);

            if (emailExists)
                return Result.Failure<User>(new Error(
                    "User.EmailExists",
                    $"A user with the email '{request.Email}' already exists.",
                    ErrorType.Conflict));

            bool locationExists = context.Locations.Any(l => l.Id == request.LocationId);

            if (!locationExists)
                return Result.Failure<User>(new Error(
                    "User.LocationNotFound",
                    $"The location with Id '{request.LocationId}' was not found.",
                    ErrorType.NotFound));

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                LocationId = request.LocationId,
                KeycloakId = request.KeycloakId
            };

            user.StripeCustomerId = request.StripeCustomerId ?? user.StripeCustomerId;

            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(user);
        }
    }
}
