using Application.Abstractions.Messaging;

namespace Application.Billing.UserServices.Delete;

public sealed record DeleteUserServiceCommand(Guid UserServiceId) : ICommand;