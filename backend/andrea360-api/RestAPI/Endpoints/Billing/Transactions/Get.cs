using FastEndpoints;
using RestAPI.Infrastructure;
using RestAPI.Extensions;
using MediatR;
using Application.Billing.Transactions.Get;

namespace RestAPI.Endpoints.Billing.Transactions;

public sealed class Get : EndpointWithoutRequest
{
    private readonly ISender _sender;

    public Get(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/transactions");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetTransactionsQuery();

        var result = await _sender.Send(query, ct);

        var response = result.Match(
            Results.Ok,
            CustomResults.Problem
        );

        await response.ExecuteAsync(HttpContext);
    }
}
