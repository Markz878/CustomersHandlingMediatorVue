namespace API.Installers;

public interface IInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration);
}
