using Application.Locations.Countries.GetById;
using Application.Locations.Countries.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Countries
{
    public sealed record GetByIdRequest(Guid Id);

    public class GetById : Endpoint<GetByIdRequest, GetCountryResponse>
    {
        private readonly ISender _sender;

        public GetById(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/countries/{id}");
        }

        public override async Task HandleAsync(GetByIdRequest req, CancellationToken ct)
        {
            var query = new GetCountryByIdQuery(req.Id);

            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.Ok,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
