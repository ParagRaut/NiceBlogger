using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NiceBlogger.Infrastructure.Caching;
using NiceBlogger.Infrastructure.Data;
using NiceBlogger.Infrastructure.Repository;
using NiceBlogger.UseCases.AuthorUseCases.Repositories;
using NiceBlogger.UseCases.PostUseCases.Repositories;

namespace NiceBlogger.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddPersistance(configuration)
            .AddDistributedCache(configuration);

        return services;
    }

    private static IServiceCollection AddPersistance(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var dbConnectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>(
            options => options.UseNpgsql(dbConnectionString));

        services.AddScoped<IPostRepository, PostRepository>();
        services.Decorate<IPostRepository, CachedPostRepository>();

        services.AddScoped<IAuthorRepository, AuthorRepository>();

        return services;
    }

    private static IServiceCollection AddDistributedCache(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var cacheConnectionString = configuration.GetConnectionString("Cache");

        services.AddStackExchangeRedisCache(
            options =>
                options.Configuration = cacheConnectionString);

        return services;
    }
}
