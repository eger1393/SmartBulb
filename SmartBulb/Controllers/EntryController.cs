
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBulb.TpLinkApi.Abstract;
using SmartBulb.TpLinkApi.Implementation;
using SmartBulb.TpLinkApi.Models;

namespace SmartBulb.Controllers
{
    [Route("api")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly ITpLink _tpLink;

        public EntryController(ITpLink tpLink)
        {
            _tpLink = tpLink;
        }

        [HttpGet("devices")]
        public async Task<IActionResult> GetDeviceList()
        {
            var res = await _tpLink.GetDeviceList();
            return Ok(res);
        }

        [HttpGet("getState")]
        public async Task<IActionResult> GetState([FromQuery] string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
                return BadRequest("deviceId con`n be null or empty");
            await _tpLink.GetDeviceState(deviceId);
            return Ok();
        }
        
        [HttpPost("setState")]
        public async Task<IActionResult> SetState([FromBody]SetStateTask model)
        {
            if (model.State.Hue != null &&
                (model.State.Hue > 360 || model.State.Hue < 0))
                return BadRequest("hue: 0-360");
            
            if (model.State.Brightness != null &&
                (model.State.Brightness > 100 || model.State.Brightness < 0))
                return BadRequest("brightness: 0-100");
            
            if (model.State.Saturation != null &&
                (model.State.Saturation > 100 || model.State.Saturation < 0))
                return BadRequest("saturation: 0-100");

            if (model.State.Power == null)
                return BadRequest("on_off: 0 or 1");
            
            await _tpLink.SetDeviceState(model.DeviceId, model.State);
            return Ok();
        }

        [HttpPost("tasks")]
        public async Task<IActionResult> Tasks([FromBody] List<SetStateTask> items)
        {
            if (items == null)
                return BadRequest("Список задач пустой");
            
            if (items.Any(item => item.WaitTime == null && item.State == null))
                return BadRequest($"Каждый элемент должен содержать в себе либо время для ожидания, либо смену состояния");

            RunTasks(items);
            return Ok();
        }
        
        [HttpPost("repeatTasks")]
        public async Task<IActionResult> RepeatTasks([FromBody] RepeatState model)
        {
            if (model.RepeatedTasks.Any(item => item.WaitTime == null && item.State == null))
                return BadRequest($"Каждый элемент должен содержать в себе либо время для ожидания, либо смену состояния");
            new Task(async () =>
            {
                if (model.StartState?.State != null)
                {
                    await _tpLink.SetDeviceState(model.StartState.DeviceId, model.StartState.State);
                    await Task.Delay((int) model.StartState.State.TransitionTime);
                }

                for(int i = 0; i < model.RepeatCount; i++)
                    await RunTasks(model.RepeatedTasks);
                
                if (model.EndState?.State != null)
                {
                    await _tpLink.SetDeviceState(model.EndState.DeviceId, model.EndState.State);
                    await Task.Delay((int) model.EndState.State.TransitionTime);
                }
            }).Start();
            return Ok();
        }

        private async Task RunTasks(List<SetStateTask> tasks)
        {
            foreach (var stateTask in tasks)
            {
                if(stateTask.WaitTime != null)
                {
                    await Task.Delay((int) stateTask.WaitTime);
                    continue;
                }
                if(stateTask.DeviceId != null)
                {
                    _tpLink.SetDeviceState(stateTask.DeviceId, stateTask.State);
                    continue;
                }
            }
        } 
    }

    public class RepeatState
    {
        [JsonProperty("startState")]
        public  SetStateTask StartState { get; set; }
        
        [JsonProperty("endState")]
        public SetStateTask EndState { get; set; }
        
        [JsonProperty("repeatedTasks")]
        [Required]
        public List<SetStateTask> RepeatedTasks { get; set; }
        
        [JsonProperty("repeatCount")]
        [Required]
        public int RepeatCount { get; set; }
    }

    public class SetStateTask
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        [JsonProperty("state")]
        public BulbState State { get; set; }
        
        [JsonProperty("waitTime")]
        public int? WaitTime { get; set; }
    }
}