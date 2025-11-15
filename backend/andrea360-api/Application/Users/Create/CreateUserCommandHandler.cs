using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Application.Users.Get;
using Domain.Users;
using Keycloak.Net;
using Microsoft.Extensions.Configuration;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

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
                return Result.Failure<UserResponse>(new Error(
                    "User.EmailExists",
                    $"A user with the email '{request.Email}' already exists.",
                    ErrorType.Conflict));

            bool locationExists = await _context.Locations.AnyAsync(l => l.Id == request.LocationId, cancellationToken);
            if (!locationExists)
                return Result.Failure<UserResponse>(new Error(
                    "User.LocationNotFound",
                    $"The location with Id '{request.LocationId}' was not found.",
                    ErrorType.NotFound));

            var keycloakUserToCreate = new Keycloak.Net.Models.Users.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                Enabled = true
            };

            bool createResult = await _keycloakClient.CreateUserAsync(_realm, keycloakUserToCreate);
            if (!createResult)
                return Result.Failure<UserResponse>(new Error(
                    "Keycloak.Error",
                    "Failed to create user in Keycloak.",
                    ErrorType.Failure));

            var keycloakUser = (await _keycloakClient.GetUsersAsync(_realm, username: request.Email))
                                .FirstOrDefault();

            if (keycloakUser is null)
                return Result.Failure<UserResponse>(new Error(
                    "Keycloak.Error",
                    "User was created but could not be retrieved from Keycloak.",
                    ErrorType.Failure));

            var localUser = new User
            {
                FirstName = keycloakUser.FirstName,
                LastName = keycloakUser.LastName,
                Email = keycloakUser.Email,
                LocationId = request.LocationId,
                KeycloakId = keycloakUser.Id,
                StripeCustomerId = request.StripeCustomerId
            };

            _context.Users.Add(localUser);
            await _context.SaveChangesAsync(cancellationToken);

            var userResponse = new UserResponse(
                Id: keycloakUser.Id,
                FirstName: keycloakUser.FirstName,
                LastName: keycloakUser.LastName,
                Email: keycloakUser.Email,
                CreatedAtTimestamp: keycloakUser.CreatedTimestamp,
                Enabled: keycloakUser.Enabled
            );

            return Result.Success(userResponse);
        }
    }
}
