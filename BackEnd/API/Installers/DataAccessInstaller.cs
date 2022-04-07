using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Installers;

public class DataAccessInstaller : IInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
    }
}
