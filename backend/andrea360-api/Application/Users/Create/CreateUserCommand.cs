using Application.Abstractions.Messaging;
using Application.Users.Get;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Users.Create
{
    public class CreateUserCommand : ICommand<UserResponse>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Guid LocationId { get; set; }

        public string? StripeCustomerId { get; set; }

        public string KeycloakId { get; set; }
    }
}
