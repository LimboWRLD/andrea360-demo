using Application.Abstractions.Messaging;
using Domain.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Billing.UserServices.Get;

public sealed record GetUserServicesQuery : IQuery<List<GetUserServiceResponse>>;
