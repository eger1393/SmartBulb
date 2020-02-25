
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartBulb.TpLinkApi.Abstract;

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
    }
}