using System.Text.Json;
using Bogus;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using NiceBlogger.Infrastructure.Data;
using NiceBlogger.UseCases.PostUseCases.Commands;

namespace Posts.FunctionalTests.Abstractions;

public abstract class FunctionalTestBase :  IClassFixture<FunctionalTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _serviceScope;

    protected FunctionalTestBase(FunctionalTestWebAppFactory webAppFactory)
    {
        _serviceScope = webAppFactory.Services.CreateScope();

        DbContext = _serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Cache = _serviceScope.ServiceProvider.GetRequiredService<IDistributedCache>();

        Sender = _serviceScope.ServiceProvider.GetRequiredService<ISender>();

        Faker = new Faker("nl");

        CreatePostCommandValidator = _serviceScope.ServiceProvider.GetRequiredService<IValidator<CreatePostCommand>>();

        HttpClient = webAppFactory.CreateClient();
        
        JsonSerializerOptions = new(JsonSerializerDefaults.Web);
    }
    
    protected ApplicationDbContext DbContext { get; }

    protected IDistributedCache Cache { get; }

    protected ISender Sender { get; }

    protected Faker Faker { get; }

    protected HttpClient HttpClient { get; }

    protected JsonSerializerOptions JsonSerializerOptions { get; }

    protected const string BaseUrl = "/api/v1/post";

    protected IValidator<CreatePostCommand> CreatePostCommandValidator { get; }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}