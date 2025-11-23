using Application.Abstractions.Messaging;
using Keycloak.Net;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Users.ResetPassword
{
    internal class ResetPasswordCommandHandler : ICommandHandler<ResetUserPasswordCommand>
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly string _realm;

        public ResetPasswordCommandHandler(KeycloakClient keycloakClient, IConfiguration configuration)
        {
            _keycloakClient = keycloakClient;
            _realm = configuration["Keycloak:Realm"]!;
        }

        public async Task<Result> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var actions = new List<string> { "UPDATE_PASSWORD" };

            var success = await _keycloakClient.SendUserUpdateAccountEmailAsync(
                _realm,
                request.UserId,
                actions,
                null,
                null,
                null,
                cancellationToken
            );

            if (!success)
            {
                return Result.Failure(new Error("Keycloak.Error","Failed to send password reset email.", ErrorType.Failure));
            }

            return Result.Success();
        }
    }
}