
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartBulb.Data.Models;
using SmartBulb.Data.Repositories.Abstract;
using SmartBulb.TpLinkApi.Abstract;

namespace SmartBulb.Controllers
{
    [Route("api")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly ITpLink _tpLink;
        private readonly IScriptRepository _scriptRepository;

        public EntryController(ITpLink tpLink, IScriptRepository scriptRepository)
        {
	        _tpLink = tpLink;
	        _scriptRepository = scriptRepository;
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
            await _tpLink.SetDeviceState(userId,model.DeviceId, model.State);
            return Ok();
        }

        [HttpPost("{userId}/tasks")]
        public IActionResult Tasks([FromRoute]Guid userId, [FromBody] List<SetStateTask> items,[FromQuery] int count = 1)
        {
            if (items == null)
                return BadRequest("Список задач пустой");
            
            if (items.Any(item => item.WaitTime == null && item.State == null))
                return BadRequest($"Каждый элемент должен содержать в себе либо время для ожидания, либо смену состояния");

            new Task(async () =>
            {
	            for (int i = 0; i < count; i++)
	            {
		            foreach (var stateTask in items)
		            {
			            if (stateTask.WaitTime != null)
			            {
				            await Task.Delay((int)stateTask.WaitTime);
				            continue;
			            }
			            if (stateTask.DeviceId != null)
			            {
#pragma warning disable 4014
				            _tpLink.SetDeviceState(userId,stateTask.DeviceId, stateTask.State);
#pragma warning restore 4014
				            continue;
			            }
		            }
                }
            }).Start();
            return Ok();
        }
      
        private async Task RunTasks(List<SetStateTask> tasks)
        {
           
        } 
    }
}