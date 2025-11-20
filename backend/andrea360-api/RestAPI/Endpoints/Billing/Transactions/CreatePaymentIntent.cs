using Application.Billing.Transactions.Create;
using Application.Billing.Transactions.CreatePaymentIntent;
using Application.Billing.Transactions.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Billing.Transactions
{
    public sealed class CreateIntentRequest
    {
        public Guid ServiceId { get; set; }

        public Guid UserId { get; set; }
    }

    public sealed class CreatePaymentIntent : Endpoint<CreateIntentRequest, string>
    {
        private readonly ISender _sender;

        public CreatePaymentIntent(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/transactions/create-intent");
        }

        public override async Task HandleAsync(CreateIntentRequest req, CancellationToken ct)
        {
            var command = new CreatePaymentIntentCommand
            {
                UserId = req.UserId,
                ServiceId = req.ServiceId,
            };

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                clientSecret => Results.Ok(new { ClientSecret = clientSecret }),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
