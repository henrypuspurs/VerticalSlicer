using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using VerticalSlicer.WebApi.Contracts;

namespace VerticalSlicer.WebApi;

public static class DependencyInjection
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddFluentValidation();
        builder.Services.AddRequestHandlers();
        builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
    }

    private static void AddRequestHandlers(this IServiceCollection services)
    {
        var handlers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(h => h.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

        foreach (var handler in handlers)
        {
            services.AddScoped(handler);
        }
    }

    private static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(config => { config.DisableDataAnnotationsValidation = true; })
            .AddValidatorsFromAssemblyContaining<Program>();
    }
}