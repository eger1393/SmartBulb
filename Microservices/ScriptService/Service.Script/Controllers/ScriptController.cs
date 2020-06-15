using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Service.Script.Data.Repositories.Abstract;
using Service.Script.Services;

namespace Service.Script.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScriptController : ControllerBase
    {
	    private readonly IScriptRepository _scriptRepository;
	    private readonly IScripExecutor _scripExecutor;

		public ScriptController(IScriptRepository scriptRepository, IScripExecutor scripExecutor)
		{
			_scriptRepository = scriptRepository;
			_scripExecutor = scripExecutor;
		}


		/// <summary>
		/// Запускает переданный скрипт немедленно
		/// полезно использовать для проверки срипта
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("run")]
		public IActionResult RunScript([FromBody]Data.Models.Script model)
	    {
		    if (string.IsNullOrEmpty(model.User?.Login) || string.IsNullOrEmpty(model.User?.Password))
			    return BadRequest("Задайте пользователя из под которого запустится скрипт");
		    _ = _scripExecutor.StartScript(model);
			return Ok("Скрипт начал свою работу");
	    }

		/// <summary>
		/// Добавляет новый скрипт в БД
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
	    [HttpPost("add")]
	    public IActionResult AddScript([FromBody]Data.Models.Script model)
		{
			if (model.StartHour == null || model.StartMinute == null)
				return BadRequest("Задайте время запуска скрипта");
			if (string.IsNullOrEmpty(model.User?.Login) || string.IsNullOrEmpty(model.User?.Password))
				return BadRequest("Задайте пользователя из под которого запустится скрипт");
			_scriptRepository.Add(model);
		    return Ok();
	    }

		/// <summary>
		/// Возвращает все сохраненные скрипты
		/// </summary>
		/// <returns></returns>
	    [HttpGet("all")]
	    public IActionResult GetAll()
		{
			var scripts = _scriptRepository.GetAll().Select(x =>
			{
				// TODO не тянуть пользователей из БД
				// TODO лучше вообще подумать как не хранить пароли в бд
				x.User = null;
				return x;
			});
		    return Ok(scripts);
	    }

		/// <summary>
		/// Удаляет скрипт из БД
		/// </summary>
		/// <param name="id">Ид скрипта</param>
		/// <returns></returns>
	    [HttpDelete("{id}")]
	    public IActionResult DeleteScript([FromRoute] Guid id)
	    {
			_scriptRepository.Delete(id);
		    return Ok();
	    }

	}
}