using FluentValidation;

namespace Application.Locations.Addresses.Create
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(c => c.Street).NotEmpty();

            RuleFor(c => c.Number).NotEmpty();
        }
    }
}
