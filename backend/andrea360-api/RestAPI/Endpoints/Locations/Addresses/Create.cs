using Application.Locations.Addresses.Create;
using Application.Locations.Addresses.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Addresses
{
    public sealed class CreateRequest
    {
        public string Street { get; set; }

        public string Number { get; set; }

        public Guid CityId { get; set; }
    }

    public sealed class Create : Endpoint<CreateRequest, GetAddressResponse>
    {
        private readonly ISender _sender;

        public Create(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/addresses");
            Roles("admin");
        }

        public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
        {
            var command = new CreateAddressCommand
            {
                Street = req.Street,
                Number = req.Number,
                CityId = req.CityId
            };

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                address => Results.Created($"/addresses/{address.Id}", address),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
