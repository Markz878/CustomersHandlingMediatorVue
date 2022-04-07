namespace API.Installers;

public static class InstallerExtension
{
    public static void InstallServices(this IServiceCollection services, IConfiguration configuration)
    {
        System.Collections.Generic.IEnumerable<IInstaller> installers = typeof(Startup).Assembly.ExportedTypes
            .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>();
        foreach (IInstaller installer in installers)
        {
            installer.Install(services, configuration);
        }
    }
}
