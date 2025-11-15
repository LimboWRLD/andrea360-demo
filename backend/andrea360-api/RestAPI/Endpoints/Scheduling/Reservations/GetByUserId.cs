using Application.Billing.Transactions.Get;
using Application.Scheduling.Reservations.Get;
using Application.Scheduling.Reservations.GetByUserId;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Scheduling.Reservations
{
    public sealed record GetByUserIdRequest(Guid userId);

    public class GetByUserId : Endpoint<GetByUserIdRequest, List<GetReservationResponse>>
    {
        private readonly ISender _sender;

        public GetByUserId(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/reservations/user/{userId}");
        }

        public override async Task HandleAsync(GetByUserIdRequest req, CancellationToken ct)
        {
            var query = new GetReservationByUserIdQuery(req.userId);

            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.Ok,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
