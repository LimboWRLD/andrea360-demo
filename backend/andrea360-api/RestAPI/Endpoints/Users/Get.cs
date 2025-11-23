using Application.Users.Get;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Users
{
    public class Get : EndpointWithoutRequest<IEnumerable<UserResponse>>
    {
        private readonly ISender _sender;

        public Get(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/users");
            Roles("admin");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var query = new GetAllUsersQuery();
            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.Ok,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
