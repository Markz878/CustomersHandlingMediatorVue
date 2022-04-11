namespace Core.ServiceCollectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreLayer(this IServiceCollection services)
    {
        services.AddSingleton<ICreditService, CreditService>();
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        return services;
    }
}
