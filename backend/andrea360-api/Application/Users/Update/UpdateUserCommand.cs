using Application.Abstractions.Messaging;
using Application.Users.Get;

namespace Application.Users.Update
{
    public class UpdateUserCommand : ICommand<UserResponse>
    {
        public Guid UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public bool? IsEnabled { get; set; }

        public Guid? LocationId { get; set; }

        public List<string>? Roles { get; set; }
    }
}