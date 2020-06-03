
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartBulb.Data.Models;
using SmartBulb.Data.Repositories.Abstract;
using SmartBulb.Models;
using SmartBulb.TpLinkApi.Abstract;

namespace SmartBulb.Controllers
{
	[Route("api")]
	[ApiController]
	public class EntryController : ControllerBase
	{
		private readonly ITpLink _tpLink;
		private readonly IScriptRepository _scriptRepository;
		private readonly ILogger<EntryController> _logger;

		public EntryController(ITpLink tpLink, IScriptRepository scriptRepository, ILogger<EntryController> logger)
		{
			_tpLink = tpLink;
			_scriptRepository = scriptRepository;
			_logger = logger;
		}

		[HttpGet("{userId}/devices")]
		public async Task<IActionResult> GetDeviceList([FromRoute]Guid userId)
		{
			var res = await _tpLink.GetDeviceList(userId);
			return Ok(res);
		}

		[HttpPost("{userId}/setState")]
		public async Task<IActionResult> SetState([FromRoute]Guid userId, [FromBody]SetStateTask model)
		{
			await _tpLink.SetDeviceState(userId, model.DeviceId, model.State);
			return Ok();
		}

		[HttpPost("{userId}/tasks")]
		public IActionResult Tasks([FromRoute]Guid userId, [FromBody]RunTasksApiModel model)
		{
			new Task(async () =>
			{
				for (var i = 0; i < model.Count; i++)
				{
					foreach (BaseTask stateTask in model.Tasks)
					{
						switch (stateTask)
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
								_logger.LogWarning($"{stateTask.Id} не был сконвертирован не в один из допустимых типов!");
								break;
							}
						}
					}
				}
			}).Start();
			return Ok();
		}
	}
}