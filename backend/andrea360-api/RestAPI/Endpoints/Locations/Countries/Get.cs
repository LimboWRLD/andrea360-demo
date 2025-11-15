using Application.Locations.Countries.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Countries;

public sealed class Get : EndpointWithoutRequest
{
    private readonly ISender _sender;

    public Get(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/countries");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetCountriesQuery();

        var result = await _sender.Send(query, ct);

        var response = result.Match(
            Results.Ok,
            CustomResults.Problem
        );

        await response.ExecuteAsync(HttpContext);
    }
}
