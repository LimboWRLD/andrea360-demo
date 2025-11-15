using Application.Abstractions.Messaging;
using Keycloak.Net;
using Microsoft.Extensions.Configuration;

namespace Application.Users.Disable
{
    internal sealed class DisableUserCommandHandler : ICommandHandler<DisableUserCommand>
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly string _realm;

        public DisableUserCommandHandler(KeycloakClient keycloakClient, IConfiguration configuration)
        {
            _keycloakClient = keycloakClient;
            _realm = configuration["Keycloak:Realm"]!;
        }

        public async Task<Result> Handle(DisableUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _keycloakClient.GetUserAsync(_realm, request.UserId);
            if (user is null)
                return Result.Failure<string>
                                    (new Error("User.NotFound", $"The user with the id: '{request.UserId}' was not found.", ErrorType.NotFound));

            user.Enabled = false;

            bool success = await _keycloakClient.UpdateUserAsync(_realm, request.UserId, user);
            if (!success)
                return Result.Failure<string>(new Error("Keycloak.Error", "Something went wrong", ErrorType.Failure));

            return Result.Success();
        }
    }
}

