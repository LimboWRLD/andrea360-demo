using Application.Abstractions.Messaging;
using Application.Billing.UserServices.Get;
using Domain.Billing;

namespace Application.Billing.UserServices.Create
{
    public sealed class CreateUserServiceCommand : ICommand<GetUserServiceResponse>
    {
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
    }
}
