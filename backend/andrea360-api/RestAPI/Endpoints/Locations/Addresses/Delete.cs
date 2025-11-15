using Application.Locations.Addresses.Delete;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Addresses
{
    public sealed record DeleteById(Guid Id);

    public class Delete : Endpoint<DeleteById, Guid>
    {
        private readonly ISender _sender;

        public Delete(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Delete("/addresses/{id}");
            Roles("admin");
        }

        public override async Task HandleAsync(DeleteById req, CancellationToken ct)
        {
            var query = new DeleteAddressCommand(req.Id);

            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.NoContent,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
