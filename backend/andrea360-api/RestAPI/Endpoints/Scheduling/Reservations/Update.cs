using Application.Locations.Locations.Get;
using Application.Scheduling.Reservations.Update;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Scheduling.Reservations
{
    public sealed record UpdateRequest(Guid ReservationId, Guid UserId, Guid SessionId, DateTime ReservedAt, Boolean IsCancelled);

    public class Update : Endpoint<UpdateRequest, GetLocationResponse>
    {
        private readonly ISender _sender;

        public Update(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/reservations/{id}");
        }

        public override async Task HandleAsync(UpdateRequest req, CancellationToken ct)
        {
            var command = new UpdateReservationCommand(req.ReservationId, req.UserId, req.SessionId, req.ReservedAt, req.IsCancelled);

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                updated => Results.Ok(updated),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
