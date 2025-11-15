using Application.Locations.Locations.Get;
using Application.Locations.Locations.Update;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Locations
{
    public sealed record UpdateRequest(Guid LocationId, string Name, Guid AddressId);

    public class Update : Endpoint<UpdateRequest, GetLocationResponse>
    {
        private readonly ISender _sender;

        public Update(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/locations/{id}");
            Roles("admin");
        }

        public override async Task HandleAsync(UpdateRequest req, CancellationToken ct)
        {
            var command = new UpdateLocationCommand(req.LocationId, req.Name, req.AddressId);

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                updated => Results.Ok(updated),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
