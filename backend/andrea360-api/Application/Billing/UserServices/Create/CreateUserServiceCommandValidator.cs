using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Billing.UserServices.Create
{
    public class CreateUserServiceCommandValidator : AbstractValidator<CreateUserServiceCommand>
    {
        public CreateUserServiceCommandValidator() 
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.ServiceId).NotEmpty();
        }
    }
}
