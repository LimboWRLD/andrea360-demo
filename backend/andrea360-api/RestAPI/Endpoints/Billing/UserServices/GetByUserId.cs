using Application.Billing.UserServices.GetByUserId;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Billing.UserServices
{
    public sealed record GetByUserIdRequest(Guid userId);

    public class GetByUserId : Endpoint<GetByUserIdRequest, List<GetUserServiceByUserIdQuery>>
    {
        private readonly ISender _sender;

        public GetByUserId(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/user-services/user/{userId}");
        }

        public override async Task HandleAsync(GetByUserIdRequest req, CancellationToken ct)
        {
            var query = new GetUserServiceByUserIdQuery(req.userId);

            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.Ok,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
