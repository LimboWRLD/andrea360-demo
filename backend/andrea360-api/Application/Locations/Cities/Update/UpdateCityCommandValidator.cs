using FluentValidation;

namespace Application.Locations.Cities.Update
{
    public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
    {
        public UpdateCityCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();

            RuleFor(c => c.CountryId).NotEmpty();
        }
    }
}
