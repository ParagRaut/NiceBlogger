using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NiceBlogger.Api;
using NiceBlogger.Infrastructure.Data;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace Posts.FunctionalTests.Abstractions;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("posts")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly RedisContainer _cacheContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(
            services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                services.AddDbContext<ApplicationDbContext>(
                    dbOptions =>
                    {
                        dbOptions.UseNpgsql(_dbContainer.GetConnectionString())
                            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    });

                services.RemoveAll(typeof(RedisCacheOptions));

                services.AddStackExchangeRedisCache(
                    cacheOptions =>
                        cacheOptions.Configuration = _cacheContainer.GetConnectionString());
            });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        await _cacheContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();

        await _cacheContainer.StopAsync();
    }
}

