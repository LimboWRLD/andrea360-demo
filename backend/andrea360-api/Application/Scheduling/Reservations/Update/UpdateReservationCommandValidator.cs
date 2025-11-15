using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduling.Reservations.Update
{
    public class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
    {
        public UpdateReservationCommandValidator()
        {
            RuleFor(c => c.ReservationId).NotEmpty();
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.SessionId).NotEmpty();
            RuleFor(c => c.ReservedAt).NotEmpty();
            RuleFor(c => c.IsCancelled).NotNull();
        }
    }
}
