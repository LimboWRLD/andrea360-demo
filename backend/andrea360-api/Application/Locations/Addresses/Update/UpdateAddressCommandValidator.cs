using FluentValidation;

namespace Application.Locations.Addresses.Update
{
    public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressCommandValidator()
        {
            RuleFor(c => c.Street).NotEmpty();

            RuleFor(c => c.Number).NotEmpty();

            RuleFor(c => c.CityId).NotEmpty();

            RuleFor(c => c.AddressId).NotEmpty();
        }
    }
}
