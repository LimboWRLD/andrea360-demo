using FastEndpoints;
using MediatR;
using RestAPI.Infrastructure;
using RestAPI.Extensions;
using Application.Billing.Transactions.Delete;

namespace RestAPI.Endpoints.Billing.Transactions;

public sealed record DeleteById(Guid Id);

public class Delete : Endpoint<DeleteById>
{
    private readonly ISender _sender;

    public Delete(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Delete("/transactions/{id}");
        Roles("admin");
    }

    public override async Task HandleAsync(DeleteById req, CancellationToken ct)
    {
        var command = new DeleteTransactionCommand(req.Id);

        var result = await _sender.Send(command, ct);

        var response = result.Match(
            Results.NoContent,
            CustomResults.Problem
        );

        await response.ExecuteAsync(HttpContext);
    }
}
