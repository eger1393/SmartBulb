using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TpLinkApi.Implementation;
using TpLinkApi.Implementation.Models;

namespace Service.TpLinkApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
	    private readonly ITpLink _tpLink;

	    public DeviceController(ITpLink tpLink)
	    {
		    _tpLink = tpLink;
	    }

		/// <summary>
		/// Получает список всех доступных устройств
		/// </summary>
		/// <param name="tpLinkToken">Токен авторизации</param>
		/// <returns>список устройств</returns>
		/// <response code="200"></response>   
		[HttpGet("list")]
		[ProducesResponseType(typeof(List<Device>), 200)]
		public async Task<IActionResult> GetList([FromHeader] string tpLinkToken)
	    {
		    var devices = await _tpLink.GetDeviceList(tpLinkToken);
		    return Ok(devices);
	    }

		/// <summary>
		/// Устанавливает новое состояние
		/// </summary>
		/// <param name="tpLinkToken">Токен авторизации</param>
		/// <param name="deviceId">ИД устройства</param>
		/// <param name="state">Новое состояние</param>
		/// <returns></returns>
		[HttpPost("{deviceId}/setState")]
		public async Task<IActionResult> SetState([FromHeader] string tpLinkToken, [FromRoute] string deviceId, [FromBody] BulbState state)
		{
			await _tpLink.SetDeviceState(tpLinkToken, deviceId, state);
			return Ok();
		}

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//[HttpGet]
		//public IActionResult GetState()
		//{
		//	return Ok();
		//}
    }
}