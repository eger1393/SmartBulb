using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SmartBulb.Data.Repositories.Abstract;
using SmartBulb.TpLinkApi.Abstract;

namespace SmartBulb.Services
{
	public class StartScriptService : IHostedService, IDisposable
	{
		private readonly IScriptRepository _scriptRepository;
		private readonly ITpLink _tpLink;
		private Timer timer;

		public StartScriptService(IScriptRepository scriptRepository, ITpLink tpLink)
		{
			_scriptRepository = scriptRepository;
			_tpLink = tpLink;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			timer = new Timer(Tick, null, TimeSpan.FromSeconds(60 -DateTime.Now.Second), TimeSpan.FromMinutes(1));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			timer.Change(Timeout.Infinite, Timeout.Infinite);
			return Task.CompletedTask;
		}

		public void Tick(object obj)
		{
			var scripts = _scriptRepository.GetAll();
			var currenTime = DateTime.Now;
			foreach (var script in scripts)
			{
				if (script.StartHour == currenTime.Hour && script.StartMinute == currenTime.Minute)
					_tpLink.StartScript(script);
			}

		}

		public void Dispose()
		{
			timer?.Dispose();
		}
	}
}