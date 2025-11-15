using Application.Locations.Countries.Get;
using Application.Locations.Countries.Update;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Countries
{
    public sealed record UpdateRequest(Guid Id, string Name);

    public class Update : Endpoint<UpdateRequest, GetCountryResponse>
    {
        private readonly ISender _sender;

        public Update(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/countries/{id}");
            Roles("admin");
        }

        public override async Task HandleAsync(UpdateRequest req, CancellationToken ct)
        {
            var command = new UpdateCountryCommand(req.Id, req.Name);

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                updated => Results.Ok(updated),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
