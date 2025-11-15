using Application.Catalog.Services.Create;
using Application.Catalog.Services.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Catalog.Services
{
    public sealed class CreateRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public sealed class Create : Endpoint<CreateRequest, GetServiceResponse>
    {
        private readonly ISender _sender;

        public Create(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/services");
            Roles("admin", "employee");
        }

        public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
        {
            var command = new CreateServiceCommand
            {
                Name = req.Name,
                Price = req.Price,
            };

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                dto => Results.Created($"/services/{dto.Id}", dto),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
