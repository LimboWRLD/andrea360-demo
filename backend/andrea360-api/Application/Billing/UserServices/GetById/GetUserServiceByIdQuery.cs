using Application.Abstractions.Messaging;
using Application.Billing.UserServices.Get;

namespace Application.Billing.UserServices.GetById;

public sealed record GetUserServiceByIdQuery(Guid UserServiceId) : IQuery<GetUserServiceResponse>;