using FluentValidation;

namespace Application.Locations.Locations.Create
{
    public class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();

            RuleFor(c => c.AddressId).NotEmpty();
        }
    }
}
