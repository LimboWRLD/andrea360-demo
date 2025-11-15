using FluentValidation;

namespace Application.Locations.Countries.Update
{
    public class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryCommandValidator()
        {
            RuleFor(c => c.CountryId).NotEmpty();

            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
