using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HostedServices.HttpClients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Script.Data.Models;
using Service.Script.Data.Repositories.Abstract;

namespace HostedServices.Services
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

		public async void Tick(object obj)
		{
			using IServiceScope scope = _scopeFactory.CreateScope();
			var scriptRepository = scope.ServiceProvider.GetService<IScriptRepository>();
			var httpClient = scope.ServiceProvider.GetService<IScriptHttpClient>();

			IEnumerable<Script> scripts = scriptRepository.GetAll();
			DateTime currentTime = DateTime.Now;
			foreach (Script script in scripts)
			{
				if (script.StartHour == currentTime.Hour && script.StartMinute == currentTime.Minute)
					await httpClient.RunScriptAsync(script);
			}
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}