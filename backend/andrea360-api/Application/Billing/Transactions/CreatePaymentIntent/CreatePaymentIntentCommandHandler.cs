using Application.Abstractions.Interfaces;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Billing.Transactions.CreatePaymentIntent
{
    internal sealed class CreatePaymentIntentCommandHandler(IApplicationDbContext context, IConfiguration configuration) : ICommandHandler<CreatePaymentIntentCommand, string>
    {
        public async Task<Result<string>> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
            var service = await context.Services.FindAsync(request.ServiceId);
            if (service == null) return Result.Failure<string>(new Error("Payment.ServiceNotFound", $"The service was not found.", ErrorType.NotFound));


            var apiKey = configuration["Stripe:SecretKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                return Result.Failure<string>(new Error("Payment.ConfigMissing", "Stripe Secret Key is missing.", ErrorType.NotFound));
            StripeConfiguration.ApiKey = apiKey;

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(service.Price * 100m),
                Currency = "eur",
                Description = $"Payment for service {service.Name}",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
                Metadata = new Dictionary<string, string>
        {
            { "ServiceId", service.Id.ToString() },
            { "MemberId", request.UserId.ToString() }
        }
            };

            var paymentService = new PaymentIntentService();
            var paymentIntent = await paymentService.CreateAsync(options, null, cancellationToken);

            return Result.Success(paymentIntent.ClientSecret);
        }
    }
}
