using Core.ServiceCollectionExtensions;

namespace API.Installers;

public class CoreServicesInstaller : IInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCoreLayer();
    }
}
