using Application.Abstractions.Behaviors;
using Application.Common.Mapping;
using FluentValidation;
using Keycloak.Net;
using Mapster;
using MapsterMapper;
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
                configuration["Keycloak:AuthClientSecret"]!,
                new KeycloakOptions(
                    adminClientId: configuration["Keycloak:AuthClientId"]!,
                    authenticationRealm: configuration["Keycloak:Realm"]!
                )
                    );
            }
            );

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(MappingConfig).Assembly);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
