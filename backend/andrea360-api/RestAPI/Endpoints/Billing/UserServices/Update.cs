using Application.Billing.UserServices.Update;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Billing.UserServices
{
    public sealed record UpdateRequest(Guid UserServiceId, Guid UserId, Guid ServiceId, int RemainingSessions);

    public class Update : Endpoint<UpdateRequest, Guid>
    {
        private readonly ISender _sender;

        public Update(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/user-services/{id}");
            Roles("admin");
        }

        public override async Task HandleAsync(UpdateRequest req, CancellationToken ct)
        {
            var command = new UpdateUserServiceCommand(req.UserServiceId, req.UserId, req.ServiceId, req.RemainingSessions);

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                updatedId => Results.Ok(updatedId),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
