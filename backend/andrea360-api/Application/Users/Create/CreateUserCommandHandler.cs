using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Users.Get;
using Domain.Users; 
using Keycloak.Net;
using Keycloak.Net.Models.Root;
using Keycloak.Net.Models.Users; 
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Users.Create
{
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly KeycloakClient _keycloakClient;
        private readonly string _realm;

        public CreateUserCommandHandler(
            IApplicationDbContext context,
            IMapper mapper,
            KeycloakClient keycloakClient,
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _keycloakClient = keycloakClient;
            _realm = configuration["Keycloak:Realm"]!;
        }

        public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (emailExists)
                return Result.Failure<UserResponse>(new Error("User.EmailExists", $"Email '{request.Email}' exists.", ErrorType.Conflict));

            var keycloakUserToCreate = new Keycloak.Net.Models.Users.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                Enabled = true,
                EmailVerified = true,

                Credentials = new List<Credentials>
                {
                    new Credentials
                    {
                        Type = "password",
                        Value = "Andrea360!", 
                        Temporary = true
                    }
                }
            };

            bool createResult = await _keycloakClient.CreateUserAsync(_realm, keycloakUserToCreate);

            if (!createResult)
                return Result.Failure<UserResponse>(new Error("Keycloak.Error", "Failed to create user in Keycloak.", ErrorType.Failure));

            var keycloakUser = (await _keycloakClient.GetUsersAsync(_realm, username: request.Email))
                                .FirstOrDefault();

            if (keycloakUser is null)
                return Result.Failure<UserResponse>(new Error("Keycloak.Error", "User created but could not be retrieved.", ErrorType.Failure));

            if (request.Roles != null && request.Roles.Any())
            {
                var allRoles = await _keycloakClient.GetRolesAsync(_realm);
                var rolesToAssign = allRoles.Where(r => request.Roles.Contains(r.Name)).ToList();

                if (rolesToAssign.Any())
                {
                    await _keycloakClient.AddRealmRoleMappingsToUserAsync(_realm, keycloakUser.Id, rolesToAssign);
                }
            }

            try
            {
                var localUser = new Domain.Users.User
                {
                    Id = Guid.NewGuid(),
                    FirstName = keycloakUser.FirstName,
                    LastName = keycloakUser.LastName,
                    Email = keycloakUser.Email,
                    LocationId = request.LocationId,
                    KeycloakId = keycloakUser.Id,
                    StripeCustomerId = request.StripeCustomerId ?? "",
                    Roles = request.Roles.ToList()
                };

                _context.Users.Add(localUser);
                await _context.SaveChangesAsync(cancellationToken);

                var userResponse = new UserResponse(
                    Id: localUser.Id,
                    FirstName: keycloakUser.FirstName,
                    LastName: keycloakUser.LastName,
                    Email: keycloakUser.Email,
                    CreatedAtTimestamp: keycloakUser.CreatedTimestamp,
                    KeycloakId: keycloakUser.Id,
                    Enabled: keycloakUser.Enabled,
                    RealmRoles: request.Roles ?? new List<string>(),
                    LocationId: localUser.LocationId
                );

                return Result.Success(userResponse);
            }
            catch (Exception)
            {
                await _keycloakClient.DeleteUserAsync(_realm, keycloakUser.Id);
                throw; 
            }
        }
    }
}