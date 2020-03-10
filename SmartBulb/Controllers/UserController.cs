using System;
using Microsoft.AspNetCore.Mvc;
using SmartBulb.Data.Models;
using SmartBulb.Data.Repositories.Abstract;

namespace SmartBulb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
	    private readonly IUserRepository _userRepository;

	    public UserController(IUserRepository userRepository)
	    {
		    _userRepository = userRepository;
	    }

		// GET api/user/all
		[HttpGet("all")]
	    public IActionResult GetAll()
	    {
		    return Ok(_userRepository.GetAll());
	    }

		// POST: api/user/add
		[HttpPost("add")]
	    public IActionResult Add(User user)
	    {
			_userRepository.Add(user);
			return Ok();
	    }

		// DELETE api/user/{userId}
		[HttpDelete("{userId}")]
	    public IActionResult Delete([FromRoute]Guid userId)
	    {
			_userRepository.Delete(userId);
			return Ok();
	    }
    }
}