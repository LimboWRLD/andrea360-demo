using Application.Abstractions.Messaging;
using Domain.Billing;

namespace Application.Billing.UserServices.GetById;

public sealed record GetUserServiceByIdQuery(Guid UserServiceId) : IQuery<UserService>;