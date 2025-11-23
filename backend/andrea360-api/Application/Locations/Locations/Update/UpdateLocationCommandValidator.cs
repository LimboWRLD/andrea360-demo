using FluentValidation;

namespace Application.Locations.Locations.Update
{
    public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
    {
        public UpdateLocationCommandValidator()
        {
            RuleFor(c => c.LocationId).NotEmpty();

            RuleFor(c => c.Name).NotEmpty();

            RuleFor(c => c.AddressId).NotEmpty();
        }
    }
}
