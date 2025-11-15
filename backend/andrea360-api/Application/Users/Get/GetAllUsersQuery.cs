using Application.Abstractions.Messaging;

namespace Application.Users.Get
{
    public sealed record UserResponse(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    long CreatedAtTimestamp,
    bool? Enabled
    );

    public sealed class GetAllUsersQuery : IQuery<IEnumerable<UserResponse>>
    {
    }
}
