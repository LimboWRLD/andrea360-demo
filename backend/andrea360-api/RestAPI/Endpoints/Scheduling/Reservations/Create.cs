using Application.Catalog.Services.Get;
using Application.Scheduling.Reservations.Create;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Scheduling.Reservations
{
    public sealed class CreateRequest
    {
        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public Boolean IsCanceled { get; set; }
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
            Post("/reservations");
        }

        public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
        {
            var command = new CreateReservationCommand
            {
                SessionId = req.SessionId,
                UserId = req.UserId,
                IsCanceled = req.IsCanceled,
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
