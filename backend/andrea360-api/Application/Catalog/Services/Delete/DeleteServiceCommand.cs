using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.Delete;

public sealed record DeleteServiceCommand(Guid ServiceId) : ICommand;
