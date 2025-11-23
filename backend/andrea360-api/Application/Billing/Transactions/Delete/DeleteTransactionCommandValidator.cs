using FluentValidation;

namespace Application.Billing.Transactions.Delete
{
    public class DeleteTransactionCommandValidator : AbstractValidator<DeleteTransactionCommand>
    {
        public DeleteTransactionCommandValidator()
        {
            RuleFor(c => c.TransactionId).NotEmpty();
        }
    }
}
