using Application.Billing.Transactions.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Infrastructure;
using RestAPI.Extensions;
using Application.Billing.UserServices.Get;

namespace RestAPI.Endpoints.Billing.UserServices
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
            Get("/user-services");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var query = new GetUserServicesQuery();

            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.Ok,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
