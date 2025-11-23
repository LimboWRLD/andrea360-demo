using Application.Locations.Locations.Get;
using Application.Locations.Locations.Update;
using Application.Scheduling.Sessions.Update;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Scheduling.Sessions
{
    public sealed record UpdateRequest(Guid Id, DateTime StartTime, DateTime EndTime, Guid LocationId, Guid ServiceId, int MaxCapacity, int CurrentCapacity);

    public class Update : Endpoint<UpdateRequest, GetLocationResponse>
    {
        private readonly ISender _sender;

        public Update(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/sessions/{id}");
            Roles("admin");
        }

        public override async Task HandleAsync(UpdateRequest req, CancellationToken ct)
        {
            var command = new UpdateSessionCommand(req.Id, req.StartTime, req.EndTime, req.LocationId, req.ServiceId, req.MaxCapacity, req.CurrentCapacity);

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                updated => Results.Ok(updated),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
