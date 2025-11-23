using Application.Catalog.Services.Delete;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Catalog.Services;

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
        Delete("/services/{id}");
        Roles("admin");
    }

    public override async Task HandleAsync(DeleteById req, CancellationToken ct)
    {
        var command = new DeleteServiceCommand(req.Id);

        var result = await _sender.Send(command, ct);

        var response = result.Match(
            Results.NoContent,
            CustomResults.Problem
        );

        await response.ExecuteAsync(HttpContext);
    }
}
