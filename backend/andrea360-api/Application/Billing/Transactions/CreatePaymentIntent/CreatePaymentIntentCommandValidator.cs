using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Billing.Transactions.CreatePaymentIntent
{
    public class CreatePaymentIntentCommandValidator : AbstractValidator<CreatePaymentIntentCommand>
    {
        public CreatePaymentIntentCommandValidator() {
            RuleFor(x => x.ServiceId).NotEmpty().WithMessage("ServiceId is required.");

            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        }
    }
}
