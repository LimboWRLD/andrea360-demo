using Application;
using Application.Hubs;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure;
using RestAPI;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPresentation(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var keycloakSettings = builder.Configuration.GetSection("Keycloak");
Console.WriteLine($"--- DEBUG KEYCLOAK ---");
Console.WriteLine($"URL: {keycloakSettings["Url"]}");
Console.WriteLine($"AdminClientId: {keycloakSettings["AdminClientId"]}");
Console.WriteLine($"AdminClientSecret: {keycloakSettings["AdminClientSecret"]}");
Console.WriteLine($"----------------------");

var app = builder.Build();

app.UseCors("AllowLocalhost4200");

await app.UseInfrastructure();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseAuthentication();
app.UseAuthorization();


app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
}).UseSwaggerGen();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
