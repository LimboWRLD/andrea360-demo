using Application.Locations.Countries.Create;
using Application.Locations.Countries.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Countries
{
    public sealed class CreateRequest
    {
        public string Name { get; set; }
    }

    public sealed class Create : Endpoint<CreateRequest, GetCountryResponse>
    {
        private readonly ISender _sender;

        public Create(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/countries");
            Roles("admin");
        }

        public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
        {
            var command = new CreateCountryCommand
            {
                Name = req.Name
            };

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                country => Results.Created($"/countries/{country.Id}", country),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
