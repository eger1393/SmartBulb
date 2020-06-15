using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostedServices.HttpClients;
using HostedServices.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Script.Data;
using Service.Script.Data.Repositories.Abstract;
using Service.Script.Data.Repositories.Implementation;

namespace HostedServices
{
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
			services.AddControllers();

			var config = Configuration.GetSection("ApplicationConfig").Get<Config>();
			services.AddSingleton(config);

			services.AddDbContext<DataContext>(x =>
					x.UseNpgsql(Configuration.GetConnectionString("default"))
						.EnableSensitiveDataLogging(),
				ServiceLifetime.Transient);
			services.AddTransient<IScriptRepository, ScriptRepository>();
			services.AddHttpClient<IScriptHttpClient, ScriptHttpClient>();
			services.AddHostedService<StartScriptService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
