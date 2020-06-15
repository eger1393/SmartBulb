using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
	    private readonly IForwardRequestService _forwardRequestService;
	    private readonly Config _config;

	    public DeviceController(IForwardRequestService forwardRequestService, Config config)
	    {
		    _forwardRequestService = forwardRequestService;
		    _config = config;
	    }

		[Route("{**route}")]
	    public async Task Index()
	    {
		    var resp = await _forwardRequestService.ForwardRequest(HttpContext, _config.TpLinkApiServiceUrl);
		    await _forwardRequestService.ForwardResponse(HttpContext, resp);
		    return;
	    }
    }
}