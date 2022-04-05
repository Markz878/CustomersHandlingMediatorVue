using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Installers;

public interface IInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration);
}
