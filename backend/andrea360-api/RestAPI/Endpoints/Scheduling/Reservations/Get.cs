using Application.Scheduling.Reservations.Get;
using Application.Scheduling.Sessions.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Scheduling.Reservations
{
    public sealed class Get : EndpointWithoutRequest
    {
        private readonly ISender _sender;

        public Get(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/reservations");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var query = new GetReservationsQuery();

            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.Ok,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
