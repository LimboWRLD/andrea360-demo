using FluentValidation;

namespace Application.Billing.UserServices.Delete
{
    public class DeleteUserServiceCommandValidator : AbstractValidator<DeleteUserServiceCommand>
    {
        public DeleteUserServiceCommandValidator()
        {
            RuleFor(c => c.UserServiceId).NotEmpty();
        }
    }
}
