using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartBulb.Data.Models;
using SmartBulb.TpLinkApi.Abstract;

namespace SmartBulb.TpLinkApi.Implementation
{
	public class TpLinkWorkService : ITpLinkWorkService
	{
		private readonly ITpLink _tpLink;
		private readonly ILogger<TpLinkWorkService> _logger;

		public TpLinkWorkService(ITpLink tpLink, ILogger<TpLinkWorkService> logger)
		{
			_tpLink = tpLink;
			_logger = logger;
		}

		public async Task StartScript(Script script)
		{
			if (script.StartState?.State != null)
			{
				await _tpLink.SetDeviceState(script.UserId, script.StartState.DeviceId, script.StartState.State);
				if (script.StartState.State.TransitionTime != null)
					await Task.Delay((int)script.StartState.State.TransitionTime);
			}

			for (int i = 0; i < script.RepeatCount; i++)
				await RunTasks(script.UserId, script.RepeatedTasks);

			if (script.EndState?.State != null)
			{
				await _tpLink.SetDeviceState(script.UserId, script.EndState.DeviceId, script.EndState.State);
				if (script.EndState.State.TransitionTime != null)
					await Task.Delay((int)script.EndState.State.TransitionTime);
			}
		}

		private async Task RunTasks(Guid userId, IEnumerable<BaseTask> tasks)
		{
			foreach (BaseTask task in tasks)
			{
				switch (task)
				{
					case WaitTask waitTask:
						await Task.Delay(waitTask.WaitTime);
						continue;
					case SetStateTask setStateTask:
#pragma warning disable 4014
						_tpLink.SetDeviceState(userId, setStateTask.DeviceId, setStateTask.State);
#pragma warning restore 4014
						break;
					default:
						{
							_logger.LogWarning($"{task.Id} не был сконвертирован не в один из допустимых типов!");
							break;
						}
				}
			}
		}
	}
}