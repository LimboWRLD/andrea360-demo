using Application.Abstractions.Behaviors;
using FluentValidation;
using Keycloak.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            });

            services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

            services.AddSignalR();

            services.AddSingleton<KeycloakClient>(sp =>
            {
                return new KeycloakClient(
                configuration["Keycloak:Url"]!,
                configuration["Keycloak:AdminUserName"]!,
                configuration["Keycloak:AdminPassword"]!,
                new KeycloakOptions(
                    adminClientId: configuration["Keycloak:AdminClientId"]!
                )
                    );
            }
            );

            return services;
        }
    }
}
