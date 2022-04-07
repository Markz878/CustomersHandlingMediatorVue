using Core.Data;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();

        CreateDdIfNotExists(host);

        host.Run();
    }

    private static void CreateDdIfNotExists(IHost host)
    {
        using IServiceScope scope = host.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        try
        {
            AppDbContext db = services.GetRequiredService<AppDbContext>();
            db.SeedData();
        }
        catch (Exception ex)
        {
            ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
