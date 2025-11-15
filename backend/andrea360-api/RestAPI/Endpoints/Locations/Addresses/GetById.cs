using Application.Locations.Addresses.Get;
using Application.Locations.Addresses.GetById;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Addresses
{
    public sealed record GetByIdRequest(Guid Id);

    public class GetById : Endpoint<GetByIdRequest, GetAddressResponse>
    {
        private readonly ISender _sender;

        public GetById(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/addresses/{id}");
        }

        public override async Task HandleAsync(GetByIdRequest req, CancellationToken ct)
        {
            var query = new GetAddressByIdQuery(req.Id);

            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.Ok,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
