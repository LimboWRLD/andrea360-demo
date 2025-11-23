using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Users.Get;
using Keycloak.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Users.Update
{
    internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly KeycloakClient _keycloakClient;
        private readonly string _realm;

        public UpdateUserCommandHandler(
            IApplicationDbContext context,
            KeycloakClient keycloakClient,
            IConfiguration configuration)
        {
            _context = context;
            _keycloakClient = keycloakClient;
            _realm = configuration["Keycloak:Realm"]!;
        }

        public async Task<Result<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var localUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (localUser == null || string.IsNullOrEmpty(localUser.KeycloakId))
                return Result.Failure<UserResponse>(new Error("User.NotFound", "Korisnik nije pronađen u lokalnoj bazi.", ErrorType.NotFound));

            if (request.LocationId.HasValue)
            {
                bool locationExists = await _context.Locations.AnyAsync(l => l.Id == request.LocationId.Value, cancellationToken);
                if (!locationExists)
                    return Result.Failure<UserResponse>(new Error("User.LocationNotFound", "Navedena lokacija ne postoji.", ErrorType.NotFound));
            }

            var keycloakUpdateModel = new Keycloak.Net.Models.Users.User
            {
                FirstName = request.FirstName ?? localUser.FirstName,
                LastName = request.LastName ?? localUser.LastName,
                Email = request.Email ?? localUser.Email,
                UserName = request.Email ?? localUser.Email,
                Enabled = request.IsEnabled ?? true 
            };

            bool keycloakUpdateSuccess = await _keycloakClient.UpdateUserAsync(_realm, localUser.KeycloakId, keycloakUpdateModel);

            if (!keycloakUpdateSuccess)
                return Result.Failure<UserResponse>(new Error("Keycloak.UpdateError", "Greška pri ažuriranju korisnika u Keycloak-u.", ErrorType.Failure));

            if (request.Roles != null)
            {
                await UpdateUserRolesAsync(localUser.KeycloakId, request.Roles);

                localUser.Roles = request.Roles.ToList();
            }

            if (request.LocationId.HasValue)
            {
                localUser.LocationId = request.LocationId.Value;
            }
            if (request.FirstName != null) localUser.FirstName = request.FirstName;
            if (request.LastName != null) localUser.LastName = request.LastName;
            if (request.Email != null) localUser.Email = request.Email;

            await _context.SaveChangesAsync(cancellationToken);

            var updatedKeycloakUser = await _keycloakClient.GetUserAsync(_realm, localUser.KeycloakId);

            var response = new UserResponse(
                localUser.Id,
                updatedKeycloakUser.FirstName,
                updatedKeycloakUser.LastName,
                updatedKeycloakUser.Email,
                updatedKeycloakUser.CreatedTimestamp,
                updatedKeycloakUser.Id,
                updatedKeycloakUser.Enabled,
                localUser.LocationId,
                RealmRoles: localUser.Roles
            );

            return Result.Success(response);
        }

        private async Task UpdateUserRolesAsync(string keycloakId, List<string> newRoles)
        {
            var allRoles = await _keycloakClient.GetRolesAsync(_realm);
            var currentRoles = await _keycloakClient.GetRealmRoleMappingsForUserAsync(_realm, keycloakId);

            var rolesToAdd = allRoles
                .Where(r => newRoles.Contains(r.Name) && !currentRoles.Any(cr => cr.Name == r.Name))
                .ToList();

            var rolesToRemove = currentRoles
                .Where(r => !newRoles.Contains(r.Name))
                .ToList();

            if (rolesToRemove.Any())
            {
                await _keycloakClient.DeleteRealmRoleMappingsFromUserAsync(_realm, keycloakId, rolesToRemove);
            }
            if (rolesToAdd.Any())
            {
                await _keycloakClient.AddRealmRoleMappingsToUserAsync(_realm, keycloakId, rolesToAdd);
            }
        }
    }
}