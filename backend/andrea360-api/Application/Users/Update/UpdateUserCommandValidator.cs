using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Update
{
    public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email must be a valid email address.")
                .When(x => !string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.Roles)
                .Must(roles => roles == null || roles.All(role => !string.IsNullOrWhiteSpace(role)))
                .WithMessage("Roles cannot contain empty or whitespace values.");
        }
    }
}
