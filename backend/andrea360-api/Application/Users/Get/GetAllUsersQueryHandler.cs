using Application.Abstractions.Messaging;
using Keycloak.Net;
using Microsoft.Extensions.Configuration;


namespace Application.Users.Get
{
    internal class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserResponse>>
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly string _realm;

        public GetAllUsersQueryHandler(KeycloakClient keycloakClient, IConfiguration configuration)
        {
            _keycloakClient = keycloakClient;
            _realm = configuration["Keycloak:Realm"]!;
        }

        public async Task<Result<IEnumerable<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _keycloakClient.GetUsersAsync(_realm);
            var response = users.Select(user => new UserResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.CreatedTimestamp,
                user.Enabled
            ));

            return await Task.FromResult(Result.Success(response));
        }
    }
}
