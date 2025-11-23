using Application.Abstractions.Messaging;
using Application.Billing.UserServices.Get;
using Domain.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Billing.UserServices.Update;

public sealed record UpdateUserServiceCommand(Guid UserServiceId, Guid UserId, Guid ServiceId, int RemainingSessions) : ICommand<GetUserServiceResponse>;
