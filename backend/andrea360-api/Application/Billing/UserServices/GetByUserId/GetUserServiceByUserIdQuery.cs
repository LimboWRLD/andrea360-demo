using Application.Abstractions.Messaging;
using Domain.Billing;

namespace Application.Billing.UserServices.GetByUserId;

public sealed record GetUserServiceByUserIdQuery(Guid UserId) : IQuery<List<UserService>>;