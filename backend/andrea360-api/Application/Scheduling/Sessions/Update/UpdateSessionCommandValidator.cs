using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Sessions.Update
{
    public class UpdateSessionCommandValidator : AbstractValidator<UpdateSessionCommand>
    {
        public UpdateSessionCommandValidator()
        {
            RuleFor(c => c.StartTime).LessThan(c => c.EndTime).WithMessage("StartTime must be earlier than EndTime.");

            RuleFor(c => c.EndTime).GreaterThan(c => c.StartTime).WithMessage("EndTime must be later than StartTime.");

            RuleFor(c => c.LocationId).NotEmpty();

            RuleFor(c => c.ServiceId).NotEmpty();

            RuleFor(c => c.MaxCapacity).GreaterThan(0);

            RuleFor(c => c.CurrentCapacity).GreaterThanOrEqualTo(0).LessThanOrEqualTo(c => c.MaxCapacity);
        }
    }
}
