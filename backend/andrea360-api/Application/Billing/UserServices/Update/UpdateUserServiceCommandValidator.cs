using FluentValidation;

namespace Application.Billing.UserServices.Update
{
    public class UpdateUserServiceCommandValidator : AbstractValidator<UpdateUserServiceCommand>
    {
        public UpdateUserServiceCommandValidator()
        {
            RuleFor(c => c.UserServiceId).NotEmpty();
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.ServiceId).NotEmpty();
            RuleFor(c => c.RemainingSessions).NotNull();
        }
    }
}
