using FluentValidation;

namespace Application.Locations.Addresses.Delete
{
    public class DeleteAddressCommandValidator : AbstractValidator<DeleteAddressCommand>
    {
        public DeleteAddressCommandValidator()
        {
            RuleFor(a => a.AddressId).NotEmpty();
        }
    }
}
