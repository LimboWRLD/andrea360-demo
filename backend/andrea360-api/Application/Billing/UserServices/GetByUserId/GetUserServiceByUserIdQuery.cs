using Application.Abstractions.Messaging;
using Application.Billing.UserServices.Get;

namespace Application.Billing.UserServices.GetByUserId;

public sealed record GetUserServiceByUserIdQuery(Guid UserId) : IQuery<List<GetUserServiceResponse>>;