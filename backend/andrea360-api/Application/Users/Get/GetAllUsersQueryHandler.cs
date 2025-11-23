using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Keycloak.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Domain.Users;
using System.Linq;

namespace Application.Users.Get
{
    internal class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserResponse>>
    {
        private readonly KeycloakClient _keycloakClient;
        private readonly IApplicationDbContext _context;
        private readonly string _realm;

        public GetAllUsersQueryHandler(
            KeycloakClient keycloakClient,
            IConfiguration configuration,
            IApplicationDbContext context)
        {
            _keycloakClient = keycloakClient;
            _realm = configuration["Keycloak:Realm"]!;
            _context = context;
        }

        public async Task<Result<IEnumerable<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var keycloakUsers = await _keycloakClient.GetUsersAsync(_realm);

            if (keycloakUsers == null || !keycloakUsers.Any())
            {
                return Result.Success(Enumerable.Empty<UserResponse>());
            }

            var keycloakIds = keycloakUsers.Select(u => u.Id).ToList();

            var localUsersData = await _context.Users
                .Where(u => keycloakIds.Contains(u.KeycloakId) && !u.IsDeleted)
                .Select(u => new
                {
                    u.Id,
                    u.KeycloakId,
                    u.LocationId,
                    u.Roles
                })
                .ToListAsync(cancellationToken);

            var locationLookup = localUsersData.ToDictionary(
                u => u.KeycloakId,
                u => new { u.Id,u.LocationId, u.Roles }
            );

            var response = keycloakUsers.Select(keycloakUser =>
            {
                locationLookup.TryGetValue(keycloakUser.Id, out var localData);

                return new UserResponse(
                    Id: localData?.Id ?? Guid.Empty,
                    FirstName: keycloakUser.FirstName,
                    LastName: keycloakUser.LastName,
                    Email: keycloakUser.Email,
                    CreatedAtTimestamp: keycloakUser.CreatedTimestamp,
                    KeycloakId: keycloakUser.Id,
                    Enabled: keycloakUser.Enabled,
                    LocationId: localData?.LocationId ?? Guid.Empty,
                    RealmRoles: localData?.Roles ?? new List<string>()
                );
            });

            return Result.Success(response);
        }
    }
}