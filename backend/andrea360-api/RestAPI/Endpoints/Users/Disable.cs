using Application.Users.Disable;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Users
{
    public class Disable : EndpointWithoutRequest
    {
        private readonly ISender _sender;

        public Disable(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/users/{UserId}/disable");
            Roles("admin");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = Route<string>("UserId");


            var command = new DisableUserCommand(
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
