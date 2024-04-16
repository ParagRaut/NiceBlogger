using Bogus;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using NiceBlogger.Infrastructure.Data;
using NiceBlogger.UseCases.PostUseCases.Commands;

namespace Posts.IntegrationTests.Abstractions;

public abstract class IntegrationTestBase : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _serviceScope;

    protected IntegrationTestBase(IntegrationTestWebAppFactory webAppFactory)
    {
        _serviceScope = webAppFactory.Services.CreateScope();

        DbContext = _serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Cache = _serviceScope.ServiceProvider.GetRequiredService<IDistributedCache>();

        Sender = _serviceScope.ServiceProvider.GetRequiredService<ISender>();

        Faker = new Faker("nl");

        CreatePostCommandValidator = _serviceScope.ServiceProvider.GetRequiredService<IValidator<CreatePostCommand>>();
    }

    protected ApplicationDbContext DbContext { get; }

    protected IDistributedCache Cache { get; }

    protected ISender Sender { get; }

    protected Faker Faker { get; }

    protected IValidator<CreatePostCommand> CreatePostCommandValidator { get; }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}