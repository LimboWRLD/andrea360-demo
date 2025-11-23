using FluentValidation;

namespace Application.Locations.Locations.Delete
{
    public class DeleteLocationCommandValidator : AbstractValidator<DeleteLocationCommand>
    {
        public DeleteLocationCommandValidator()
        {
            RuleFor(c => c.LocationId).NotEmpty();
        }
    }
}
