using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartBulb.Data.Models;
using SmartBulb.Data.Repositories.Abstract;
using SmartBulb.TpLinkApi.Abstract;

namespace SmartBulb.Services
{
	public class StartScriptService : IHostedService, IDisposable
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private Timer _timer;

		public StartScriptService(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_timer = new Timer(Tick, null, TimeSpan.FromSeconds(60 - DateTime.Now.Second), TimeSpan.FromMinutes(1));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer.Change(Timeout.Infinite, Timeout.Infinite);
			return Task.CompletedTask;
		}

		public void Tick(object obj)
		{
			using IServiceScope scope = _scopeFactory.CreateScope();
			var scriptRepository = scope.ServiceProvider.GetService<IScriptRepository>();
			var tpLinkWorkService = scope.ServiceProvider.GetService<ITpLinkWorkService>();

			IEnumerable<Script> scripts = scriptRepository.GetAll();
			DateTime currentTime = DateTime.Now;
			foreach (Script script in scripts)
			{
				if (script.StartHour == currentTime.Hour && script.StartMinute == currentTime.Minute)
					tpLinkWorkService.StartScript(script);
			}
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}