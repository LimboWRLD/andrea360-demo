using Application.Billing.UserServices.GetById;
using FastEndpoints;
using MediatR;
using RestAPI.Extensions;
using RestAPI.Infrastructure;

namespace RestAPI.Endpoints.Billing.UserServices
{
    public sealed record GetByIdRequest(Guid Id);

    public class GetById : Endpoint<GetByIdRequest, GetUserServiceByIdQuery>
    {
        private readonly ISender _sender;

        public GetById(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Get("/user-services/{id}");
        }

        public override async Task HandleAsync(GetByIdRequest req, CancellationToken ct)
        {
            var query = new GetUserServiceByIdQuery(req.Id);

            var result = await _sender.Send(query, ct);

            var response = result.Match(
                Results.Ok,
                CustomResults.Problem
            );

            await response.ExecuteAsync(HttpContext);
        }
    }
}
