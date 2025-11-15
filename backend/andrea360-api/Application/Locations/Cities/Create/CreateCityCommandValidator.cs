using FluentValidation;

namespace Application.Locations.Cities.Create
{
    public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();

            RuleFor(c => c.CountryId).NotEmpty();
        }
    }
}
