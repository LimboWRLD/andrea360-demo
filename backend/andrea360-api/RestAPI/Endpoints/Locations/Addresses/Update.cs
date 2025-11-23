using Application.Locations.Addresses.Get;
using Application.Locations.Addresses.Update;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace WebApi.Endpoints.Locations.Addresses
{
    public sealed record UpdateRequest(Guid Id, Guid CityId, string Street, string Number);

    public class Update : Endpoint<UpdateRequest, GetAddressResponse>
    {
        private readonly ISender _sender;

        public Update(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/addresses/{id}");
            Roles("admin");
        }

        public override async Task HandleAsync(UpdateRequest req, CancellationToken ct)
        {
            var command = new UpdateAddressCommand(req.Id, req.CityId, req.Street, req.Number);

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                updated => Results.Ok(updated),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
