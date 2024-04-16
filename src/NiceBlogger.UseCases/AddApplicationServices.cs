using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NiceBlogger.UseCases.Common.Behaviours;

namespace NiceBlogger.UseCases;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ApplicationServiceExtensions).Assembly);

            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationServiceExtensions).Assembly);

        return services;
    }
}