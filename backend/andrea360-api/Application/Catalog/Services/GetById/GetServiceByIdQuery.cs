using Application.Abstractions.Messaging;
using Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.GetById;

public sealed record GetServiceByIdQuery(Guid ServiceId) : IQuery<Service>;
