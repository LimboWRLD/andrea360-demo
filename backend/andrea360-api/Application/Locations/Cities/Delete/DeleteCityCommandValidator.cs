using FluentValidation;

namespace Application.Locations.Cities.Delete
{
    public class DeleteCityCommandValidator : AbstractValidator<DeleteCityCommand>
    {
        public DeleteCityCommandValidator()
        {
            RuleFor(c => c.CityId).NotEmpty();
        }
    }
}
