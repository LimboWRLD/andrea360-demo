using FluentValidation;

namespace Application.Billing.Transactions.Update
{
    public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
    {
        public UpdateTransactionCommandValidator() 
        {
            RuleFor(c => c.TransactionId).NotEmpty();

            RuleFor(c => c.UserId).NotEmpty();

            RuleFor(c => c.Amount).GreaterThan(0);

            RuleFor(c => c.TransactionDate).NotEmpty();

            RuleFor(c => c.StripeTransactionId).NotEmpty();
        }
    }
}
