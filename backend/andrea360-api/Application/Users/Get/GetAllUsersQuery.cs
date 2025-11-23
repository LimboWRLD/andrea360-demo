using Application.Abstractions.Messaging;

namespace Application.Users.Get
{
    public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    long CreatedAtTimestamp,
    String KeycloakId,
    bool? Enabled,
    Guid LocationId,
    IEnumerable<string>RealmRoles
    );

    public sealed class GetAllUsersQuery : IQuery<IEnumerable<UserResponse>>
    {
    }
}
