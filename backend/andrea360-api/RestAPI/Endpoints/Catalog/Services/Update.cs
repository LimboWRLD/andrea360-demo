using Application.Catalog.Services.Update;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Catalog.Services
{
    public sealed record UpdateRequest(Guid ServiceId, string Name, decimal Price);

    public class Update : Endpoint<UpdateRequest, Guid>
    {
        private readonly ISender _sender;

        public Update(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/services/{id}");
            Roles("admin", "employee");
        }

        public override async Task HandleAsync(UpdateRequest req, CancellationToken ct)
        {
            var command = new UpdateServiceCommand(req.ServiceId, req.Name, req.Price);

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                updatedId => Results.Ok(updatedId),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
