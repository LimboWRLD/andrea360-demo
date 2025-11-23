using Application.Locations.Cities.Create;
using Application.Locations.Cities.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Locations.Cities
{
    public sealed class CreateRequest
    {
        public string Name { get; set; }

        public Guid CountryId { get; set; }
    }

    public sealed class Create : Endpoint<CreateRequest, GetCityResponse>
    {
        private readonly ISender _sender;

        public Create(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/cities");
            Roles("admin");
        }

        public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
        {
            var command = new CreateCityCommand
            {
                Name = req.Name,
                CountryId = req.CountryId
            };

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                city => Results.Created($"/cities/{city.Id}", city),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
