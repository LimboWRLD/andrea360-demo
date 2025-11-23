using FluentValidation;

namespace Application.Locations.Countries.Create
{
    public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
