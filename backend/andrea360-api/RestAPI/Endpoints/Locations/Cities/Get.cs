using Application.Locations.Cities.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Cities;

public sealed class Get : EndpointWithoutRequest
{
    private readonly ISender _sender;

    public Get(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/cities");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetCitiesQuery();

        var result = await _sender.Send(query, ct);

        var response = result.Match(
            Results.Ok,
            CustomResults.Problem
        );

        await response.ExecuteAsync(HttpContext);
    }
}
