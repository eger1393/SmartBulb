using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.TpLinkApi.Models;
using TpLinkApi.Implementation;

namespace Service.TpLinkApi.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
	    private readonly ITpLink _tpLink;

	   
	    public AuthorizeController(ITpLink tpLink)
	    {
		    _tpLink = tpLink;
	    }

		/// <summary>
		/// Авторизует пользователя
		/// </summary>
		/// <param name="model"></param>
		/// <returns>Токен авторизации</returns>
		[HttpPost]
	    public async Task<IActionResult> Authorize([FromBody] AuthorizeRequest model)
		{
			string token = await _tpLink.Authorize(model.Login, model.Password);
			return Ok(token);
		}
    }
}