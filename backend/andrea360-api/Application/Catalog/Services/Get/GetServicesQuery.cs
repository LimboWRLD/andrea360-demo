using Application.Abstractions.Messaging;
using Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.Get;

public sealed record GetServicesQuery : IQuery<List<GetServiceResponse>>;
