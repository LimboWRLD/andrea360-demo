using Application.Abstractions.Messaging;
using Application.Catalog.Services.Get;
using Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.GetById;

public sealed record GetServiceByIdQuery(Guid ServiceId) : IQuery<GetServiceResponse>;
