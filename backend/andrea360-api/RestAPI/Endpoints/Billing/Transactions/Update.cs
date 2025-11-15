using Application.Billing.Transactions.Update;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Billing.Transactions
{
    public sealed record UpdateRequest(Guid TransactionId, Guid UserId, Guid ServiceId, decimal Amount, DateTime TransactionDate, string StripeTransactionId);

    public class Update : Endpoint<UpdateRequest, Guid>
    {
        private readonly ISender _sender;

        public Update(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/transactions/{id}");
            Roles("admin");
        }

        public override async Task HandleAsync(UpdateRequest req, CancellationToken ct)
        {
            var command = new UpdateTransactionCommand(req.TransactionId, req.UserId, req.ServiceId, req.Amount, req.TransactionDate, req.StripeTransactionId);

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                updatedId => Results.Ok(updatedId),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
