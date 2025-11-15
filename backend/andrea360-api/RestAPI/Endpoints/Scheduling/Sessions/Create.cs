using Application.Catalog.Services.Create;
using Application.Catalog.Services.Get;
using Application.Scheduling.Sessions.Create;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Scheduling.Sessions
{
    public sealed class CreateRequest
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Guid LocationId { get; set; }

        public Guid ServiceId { get; set; }

        public int MaxCapacity { get; set; }
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
            Post("/sessions");
            Roles("admin", "employee");
        }

        public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
        {
            var command = new CreateSessionCommand
            {
                StartTime = req.StartTime,
                EndTime = req.EndTime,
                LocationId = req.LocationId,
                ServiceId = req.ServiceId,
                MaxCapacity = req.MaxCapacity
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
