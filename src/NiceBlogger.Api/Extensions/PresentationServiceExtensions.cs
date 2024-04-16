using NiceBlogger.Api.Middlewares;

namespace NiceBlogger.Api.Extensions;

public static class PresentationServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();

        services.AddTransient<ExceptionHandlingMiddleware>();
        
        return services;
    }

    public static IServiceCollection AddHealthChecks(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var dbConnectionString = configuration.GetConnectionString("Database");

        var cacheConnectionString = configuration.GetConnectionString("Cache");

        services.AddHealthChecks()
            .AddNpgSql(dbConnectionString!)
            .AddRedis(cacheConnectionString!)
            .AddSeqPublisher(
                options =>
                {
                    options.Endpoint = configuration.GetConnectionString("Seq")!;
                });

        return services;
    }
}
