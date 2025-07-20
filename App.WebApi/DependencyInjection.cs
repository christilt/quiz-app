using App.WebApi.Features.Admin;

namespace App.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddSingleton<QuizEndpointHandler>();
        return services;
    }
}
