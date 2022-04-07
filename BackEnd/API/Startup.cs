using API.Installers;
using Microsoft.OpenApi.Models;

namespace API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.InstallServices(Configuration);
        services.AddSpaStaticFiles(options => options.RootPath = "wwwroot");
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.DisplayRequestDuration(); c.SwaggerEndpoint("/swagger/v1/swagger.json", "MediatRReponseCaching v1"); });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        app.UseSpa(config =>
        {
            if (env.IsDevelopment())
            {
                config.UseProxyToSpaDevelopmentServer("http://localhost:8080");
            }
        });
    }
}
