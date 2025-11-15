using Application.Locations.Cities.Get;
using Application.Locations.Cities.Update;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Cities
{
    public sealed record UpdateRequest(Guid Id, string Name, Guid CountryId);

    public class Update : Endpoint<UpdateRequest, GetCityResponse>
    {
        private readonly ISender _sender;

        public Update(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/cities/{id}");
            Roles("admin");
        }

        public override async Task HandleAsync(UpdateRequest req, CancellationToken ct)
        {
            var command = new UpdateCityCommand(req.Id, req.CountryId, req.Name);

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                updated => Results.Ok(updated),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
