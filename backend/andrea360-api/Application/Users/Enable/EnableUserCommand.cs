using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Enable
{
    public sealed record EnableUserCommand(string UserId) : ICommand
    {
    }
}
