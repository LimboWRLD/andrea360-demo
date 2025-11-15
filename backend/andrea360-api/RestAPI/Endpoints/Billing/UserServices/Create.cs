using Application.Billing.UserServices.Create;
using Application.Billing.UserServices.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Billing.UserServices
{
    public sealed class CreateRequest
    {
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
    }

    public sealed class Create : Endpoint<CreateRequest, GetUserServiceResponse>
    {
        private readonly ISender _sender;

        public Create(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/user-services");
        }

        public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
        {
            var command = new CreateUserServiceCommand
            {
                UserId = req.UserId,
                ServiceId = req.ServiceId
            };

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                dto => Results.Created($"/user-services/{dto.Id}", dto),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
