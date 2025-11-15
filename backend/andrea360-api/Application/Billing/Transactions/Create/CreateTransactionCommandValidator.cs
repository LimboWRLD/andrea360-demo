using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Billing.Transactions.Create
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.ServiceId)
                .NotEmpty().WithMessage("ServiceId is required.");
            RuleFor(x => x.StripeTransactionId)
                .NotEmpty().WithMessage("StripeTransactionId is required.");
        }
    }
}
