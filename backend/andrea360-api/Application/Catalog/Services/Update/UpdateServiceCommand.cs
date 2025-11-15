using Application.Abstractions.Messaging;
using Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.Update;

public sealed record UpdateServiceCommand(Guid ServiceId, string Name, decimal Price) : ICommand<Service>;