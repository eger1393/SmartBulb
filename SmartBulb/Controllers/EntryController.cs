
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBulb.TpLinkApi.Abstract;
using SmartBulb.TpLinkApi.Implementation;

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
        public async Task<IActionResult> Tasks([FromBody] List<dynamic> items)
        {
            if (items == null)
                return BadRequest("Список задач пустой");
            foreach (var item in items)
            {
                var stateTask = JsonConvert.DeserializeObject<SetStateTask>(item.ToString());
                if(stateTask?.DeviceId != null)
                // if (item is SetStateTask stateTask)
                {
                    await _tpLink.SetDeviceState(stateTask.DeviceId, stateTask.State);
                    continue;
                }
                
                var waitTask = JsonConvert.DeserializeObject<WaitTask>(item.ToString());
                if(waitTask != null)
                    // if (item is WaitTask waitTask)
                {
                    await Task.Delay(waitTask.WaitTime);
                }
            }
            return Ok();
        }
    }

    public class SetStateTask : ITaskItem
    {
        [Required]
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        [Required]
        [JsonProperty("state")]
        public LightState State { get; set; }
    }

    public class WaitTask : ITaskItem
    {
        [Required]
        [JsonProperty("waitTime")]
        public int WaitTime { get; set; }
    }

    public interface ITaskItem
    {
        
    }
}