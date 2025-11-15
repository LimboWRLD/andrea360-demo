using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Create
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.LastName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(255);
            RuleFor(c => c.LocationId).NotEmpty();
            RuleFor(c => c.StripeCustomerId).MaximumLength(255);
            RuleFor(c => c.KeycloakId).NotEmpty().MaximumLength(255);
        }
    }
}
