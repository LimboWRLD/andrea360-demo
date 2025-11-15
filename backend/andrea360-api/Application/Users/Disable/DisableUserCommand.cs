using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Disable
{
    public sealed record DisableUserCommand(string UserId) : ICommand
    {
    }
}
