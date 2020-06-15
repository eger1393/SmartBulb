using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Service.Script.Data;
using Service.Script.Data.Repositories.Abstract;
using Service.Script.Data.Repositories.Implementation;
using Service.Script.HttpClients;
using Service.Script.JsonConverters;
using Service.Script.Services;

namespace Service.Script
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
			services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.Converters.Add(new BaseTaskConverter())); ;
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1",
					new OpenApiInfo
					{
						Title = "API",
						Version = "v1"
					}
				);

				var filePath = Path.Combine(System.AppContext.BaseDirectory, "Service.TpLinkApi.xml");
				c.IncludeXmlComments(filePath);
			});

			var config = Configuration.GetSection("ApplicationConfig").Get<Config>();
			services.AddSingleton(config);

			//Postgres не поддерживает MARS.
			//https://stackoverflow.com/questions/39595968/entityframework-dbcontext-lifecycle-postgres-an-operation-is-already-in-prog/39599923#39599923
			services.AddDbContext<DataContext>(x =>
					x.UseNpgsql(Configuration.GetConnectionString("default"))
						.EnableSensitiveDataLogging(),
				ServiceLifetime.Transient);
			services.AddTransient<IScriptRepository, ScriptRepository>();
			services.AddScoped<IScripExecutor, ScriptExecutor>();

			services.AddHttpClient<ITpLinkApiServiceHttpClient, TpLinkApiServiceHttpClient>();
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
			app.UseSwagger();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
			});
		}
	}
}
