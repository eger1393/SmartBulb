using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartBulb.Data;
using SmartBulb.Data.Repositories.Abstract;
using SmartBulb.Data.Repositories.Implementation;
using SmartBulb.Models;
using SmartBulb.Services;
using SmartBulb.TpLinkApi.Abstract;
using SmartBulb.TpLinkApi.HttpClients;
using SmartBulb.TpLinkApi.Implementation;

namespace SmartBulb
{
    public class Startup
    {
	    private readonly IConfiguration _configuration;

	    public Startup(IConfiguration configuration)
	    {
		    _configuration = configuration;
	    }

	    public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(x => x.SerializerSettings.Converters.Add(new BaseTaskConverter()));

            services.AddDbContext<DataContext>(x => x.UseSqlite(_configuration.GetConnectionString("default")));
            services.AddScoped<IScriptRepository, ScriptRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddHttpClient<ITpLinkHttpClient, TpLinkHttpClient>();
            services.AddScoped<ITpLinkWorkService, TpLinkWorkService>();
            services.AddScoped<ITpLink, TpLink>();

            services.AddHostedService<StartScriptService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            });
        }
    }
}