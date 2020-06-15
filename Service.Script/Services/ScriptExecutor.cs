using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.Script.Data.Models;
using Service.Script.HttpClients;

namespace Service.Script.Services
{
	public interface IScripExecutor
	{
		/// <summary>
		/// Запускает скрипт
		/// </summary>
		/// <param name="script"></param>
		/// <returns></returns>
		Task StartScript(Data.Models.Script script);
	}
	public class ScriptExecutor : IScripExecutor
	{
		private readonly ITpLinkApiServiceHttpClient _httpClient;
		private readonly ILogger<ScriptExecutor> _logger;

		public ScriptExecutor(ITpLinkApiServiceHttpClient httpClient, ILogger<ScriptExecutor> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}

		public async Task StartScript(Data.Models.Script script)
		{
			if (script.User == null)
				throw new ArgumentException("У скрипта отсутствует пользователь для которого он должен выполняться");

			if (script.RepeatedTasks.Count == 0)
				throw new ArgumentException("В скрипте отсутствуют таски");

			var token = await _httpClient.AuthorizeAsync(script.User.Login, script.User.Password);
			if (script.StartState?.State != null)
				await SetStateAndWaitTransitionTime(token, script.StartState);

			for (int i = 0; i < script.RepeatCount; i++)
				await RunTasks(token, script.RepeatedTasks);

			if (script.EndState?.State != null)
				await SetStateAndWaitTransitionTime(token, script.EndState);
		}

		private async Task SetStateAndWaitTransitionTime(string token, SetStateTask task)
		{
			await _httpClient.SetStateAsync(token, task.DeviceId, task.State);
			if (task.State.TransitionTime != null)
				await Task.Delay((int)task.State.TransitionTime);
		}

		private async Task RunTasks(string token, IEnumerable<BaseTask> tasks)
		{
			foreach (BaseTask task in tasks)
			{
				switch (task)
				{
					case WaitTask waitTask:
						await Task.Delay(waitTask.WaitTime);
						break;
					case SetStateTask setStateTask:
						// ReSharper disable once AssignmentIsFullyDiscarded
						_ = Task.Run(() => _httpClient.SetStateAsync(token, setStateTask.DeviceId, setStateTask.State));
						break;
					default:
						_logger.LogWarning($"{task.Id} не был сконвертирован не в один из допустимых типов!");
						break;

				}
			}
		}
	}
}