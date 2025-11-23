using Application.Billing.Transactions.Create;
using Application.Billing.Transactions.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Billing.Transactions
{
    public sealed class CreateRequest
    {
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
        public decimal Amount { get; set; }
        public string StripeTransactionId { get; set; }
    }

    public sealed class Create : Endpoint<CreateRequest, GetTransactionResponse>
    {
        private readonly ISender _sender;

        public Create(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/transactions");
        }

        public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
        {
            var command = new CreateTransactionCommand
            {
                UserId = req.UserId,
                ServiceId = req.ServiceId,
                Amount = req.Amount,
                StripeTransactionId = req.StripeTransactionId
            };

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                dto => Results.Created($"/transactions/{dto.Id}", dto),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
