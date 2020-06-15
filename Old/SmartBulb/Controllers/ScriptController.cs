using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBulb.Data.Models;
using SmartBulb.Data.Repositories.Abstract;

namespace SmartBulb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScriptController : ControllerBase
    {
	    private readonly IScriptRepository _scriptRepository;

	    public ScriptController(IScriptRepository scriptRepository)
	    {
		    _scriptRepository = scriptRepository;
	    }

	    [HttpPost("add")]
	    public async Task<IActionResult> AddScript([FromBody]Script script)
	    {
		    if (script.StartHour == null || script.StartMinute == null)
			    return BadRequest("Задайте время запуска скрипта");
		    if (string.IsNullOrEmpty(script.Name))
			    return BadRequest("Задайте имя скрипта");
		    if (script.UserId == Guid.Empty)
			    return BadRequest("Выберите пользователя из под которого будет выполнен скрипт");
			_scriptRepository.Add(script);
		    return Ok();
	    }

	    [HttpGet("all")]
	    public IActionResult GetAll()
	    {
		    return Ok(_scriptRepository.GetAll().Select(x => new
		    {
			    x.Id,
			    x.Name,
			    x.StartHour,
			    x.StartMinute
		    }));
	    }

	    [HttpDelete("{id}")]
	    public IActionResult DeleteScript([FromRoute] Guid id)
	    {
		    if (id == Guid.Empty)
			    return BadRequest("Не правильный id");
		    _scriptRepository.Delete(id);
		    return Ok();
	    }
    }
}