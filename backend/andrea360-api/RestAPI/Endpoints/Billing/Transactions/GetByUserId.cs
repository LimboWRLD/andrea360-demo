using Application.Billing.Transactions.Get;
using Application.Billing.Transactions.GetByUserId;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Billing.Transactions
{
    public sealed record GetByUserIdRequest(Guid userId);

    public class GetByUserId : Endpoint<GetByUserIdRequest, List<GetTransactionResponse>>
    {
        private readonly ISender _sender;

        public GetByUserId(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/transactions/user/{userId}");
        }

        public override async Task HandleAsync(GetByUserIdRequest req, CancellationToken ct)
        {
            var query = new GetTransactionsByUserIdQuery(req.userId);

            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.Ok,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
