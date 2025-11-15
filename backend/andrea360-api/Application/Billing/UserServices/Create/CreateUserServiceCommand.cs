using Application.Abstractions.Messaging;
using Domain.Billing;

namespace Application.Billing.UserServices.Create
{
    public sealed class CreateUserServiceCommand : ICommand<UserService>
    {
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
    }
}
