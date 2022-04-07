using Core.Settings;

namespace API.Installers;

public class CacheInstaller : IInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CacheSettings>(configuration.GetSection(nameof(CacheSettings)));
        services.AddDistributedMemoryCache();
    }
}
