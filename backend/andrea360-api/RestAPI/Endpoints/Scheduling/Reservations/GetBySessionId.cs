using Application.Scheduling.Reservations.Get;
using Application.Scheduling.Reservations.GetBySessionId;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Scheduling.Reservations
{
    public sealed record GetBySessionIdRequest(Guid Id);

    public class GetBySessionId : Endpoint<GetByIdRequest, GetReservationResponse>
    {
        private readonly ISender _sender;

        public GetBySessionId(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/reservations/session/{id}");
        }

        public override async Task HandleAsync(GetByIdRequest req, CancellationToken ct)
        {
            var query = new GetReservationBySessionIdQuery(req.Id);

            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.Ok,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
