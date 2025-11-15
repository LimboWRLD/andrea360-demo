using FluentValidation;

namespace Application.Locations.Countries.Delete
{
    public class DeleteCountryCommandValidator : AbstractValidator<DeleteCountryCommand>
    {
        public DeleteCountryCommandValidator()
        {
            RuleFor(c => c.CountryId).NotEmpty();
        }
    }
}
