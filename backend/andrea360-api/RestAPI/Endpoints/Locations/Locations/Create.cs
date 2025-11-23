using Application.Locations.Locations.Create;
using Application.Locations.Locations.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Locations
{
    public sealed class CreateRequest
    {
        public string Name { get; set; }

        public Guid AddressId { get; set; }
    }

    public sealed class Create : Endpoint<CreateRequest, GetLocationResponse>
    {
        private readonly ISender _sender;

        public Create(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/locations");
            Roles("admin");
        }

        public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
        {
            var command = new CreateLocationCommand
            {
                Name = req.Name,
                AddressId = req.AddressId
            };

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                location => Results.Created($"/locations/{location.Id}", location),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
