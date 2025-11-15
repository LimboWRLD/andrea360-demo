using Application.Locations.Cities.Delete;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Cities
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
            Delete("/cities/{id}");
            Roles("admin");
        }

        public override async Task HandleAsync(DeleteById req, CancellationToken ct)
        {
            var query = new DeleteCityCommand(req.Id);

            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.NoContent,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
