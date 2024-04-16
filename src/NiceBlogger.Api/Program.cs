using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NiceBlogger.Api.Endpoints.V1;
using NiceBlogger.Api.Extensions;
using NiceBlogger.Api.Middlewares;
using NiceBlogger.Infrastructure;
using NiceBlogger.UseCases;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices()
    .AddHealthChecks(builder.Configuration);

builder.Host.UseSerilog(
(context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    app.SeedData();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSerilogRequestLogging();

// Add a redirect from the root of the app to the swagger endpoint
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapGroup("/api/v1/")
    .WithTags("Posts API")
    .MapPostEndpoints();

app.Run();
