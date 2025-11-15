using Application.Users.ResetPassword;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Users
{
    public class ResetPassoword : EndpointWithoutRequest
    {
        private readonly ISender _sender;

        public ResetPassoword(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/users/{UserId}/reset-password");
            Roles("admin");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = Route<string>("UserId");

            var command = new ResetUserPasswordCommand(
                userId
            );

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                Results.NoContent,
                CustomResults.Problem
            );


            await response.ExecuteAsync(HttpContext);
        }
    }
}
