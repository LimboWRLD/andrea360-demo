using Application.Users.Create;
using Application.Users.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Users
{
    public sealed class CreateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid LocationId { get; set; }
        public string? StripeCustomerId { get; set; }

        public List<string>? RealmRoles { get; set; }
    }

    public sealed class CreateUser : Endpoint<CreateRequest, UserResponse>
    {
        private readonly ISender _sender;

        public CreateUser(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("/users");
            Roles("admin", "employee");
        }

        public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
        {
            var command = new CreateUserCommand
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
                LocationId = req.LocationId,
                StripeCustomerId = req.StripeCustomerId,
                Roles = req.RealmRoles
            };

            var result = await _sender.Send(command, ct);

            var response = result.Match(
                user => Results.Created($"/users/{user.Id}", user),
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
