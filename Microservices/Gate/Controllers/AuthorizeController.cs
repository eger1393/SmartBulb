using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gate.HttpClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gate.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorizeController : ControllerBase
	{
		private readonly ITpLinkApiHttpClient _apiHttpClient;


		public AuthorizeController(ITpLinkApiHttpClient apiHttpClient)
		{
			_apiHttpClient = apiHttpClient;
		}

		/// <summary>
		/// Авторизует пользователя
		/// </summary>
		/// <param name="model"></param>
		/// <returns>Токен авторизации</returns>
		[HttpPost]
		public async Task<IActionResult> Authorize([FromBody] AuthorizeRequest model)
		{
			string token = await _apiHttpClient.AuthorizeAsync(model.Login, model.Password);
			return Ok(token);
		}
	}

	public class AuthorizeRequest
	{
		[Required]
		public string Login { get; set; }
		[Required]
		public string Password { get; set; }
	}
}