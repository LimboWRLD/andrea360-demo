using Application.Abstractions.Messaging;
using Application.Users.Disable;
using Keycloak.Net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Enable
{
    internal class EnableUserCommandHandler : ICommandHandler<EnableUserCommand>
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly string _realm;

        public EnableUserCommandHandler(KeycloakClient keycloakClient, IConfiguration configuration)
        {
            _keycloakClient = keycloakClient;
            _realm = configuration["Keycloak:Realm"]!;
        }

        public async Task<Result> Handle(EnableUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _keycloakClient.GetUserAsync(_realm, request.UserId);
            if (user is null)
                return Result.Failure<string>
                                    (new Error("User.NotFound", $"The user with the id: '{request.UserId}' was not found.", ErrorType.NotFound));

            user.Enabled = true;

            bool success = await _keycloakClient.UpdateUserAsync(_realm, request.UserId, user);
            if (!success)
                return Result.Failure<string>(new Error("Keycloak.Error", "Something went wrong", ErrorType.Failure));

            return Result.Success();
        }
    }
}
