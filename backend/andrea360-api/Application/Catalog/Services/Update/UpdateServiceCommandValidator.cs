using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Services.Update
{
    internal class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
    {
        public UpdateServiceCommandValidator()
        {
            RuleFor(c => c.ServiceId).NotEmpty();
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Price).GreaterThan(0);
        }
    }
}
