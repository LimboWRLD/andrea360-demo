using Application.Users.Get;
using Application.Users.Update;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Users.Update
{
    public sealed record UpdateUserRequest
    (
        Guid Id,
        string? FirstName,
        string? LastName,
        string? Email,
        bool? IsEnabled,
        Guid? LocationId,
        List<string>? Roles
    );

    public class UpdateUserEndpoint : Endpoint<UpdateUserRequest, UserResponse>
    {
        private readonly ISender _sender;

        public UpdateUserEndpoint(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Put("/users/{id}");
            Roles("admin");
        }

        public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
        {
            var command = new UpdateUserCommand
            {
                UserId = req.Id,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                IsEnabled = req.IsEnabled,
                LocationId = req.LocationId,
                Roles = req.Roles
            };

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                updated => Results.Ok(updated),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}